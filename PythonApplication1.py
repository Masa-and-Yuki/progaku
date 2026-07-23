# -*- coding: utf-8 -*-
import cv2
import time
import mediapipe as mp
from pynput.keyboard import Controller, Key

from mediapipe.tasks import python
from mediapipe.tasks.python import vision

# ===== keyboard =====
keyboard = Controller()
pressed_keys = set()


def press_key(k):
    if k not in pressed_keys:
        keyboard.press(k)
        pressed_keys.add(k)


def release_all():
    for k in list(pressed_keys):
        keyboard.release(k)
    pressed_keys.clear()


# ===== model =====
base_options = python.BaseOptions(
    model_asset_path="hand_landmarker.task"
)

options = vision.HandLandmarkerOptions(
    base_options=base_options,
    running_mode=vision.RunningMode.VIDEO,
    num_hands=2,
    min_hand_detection_confidence=0.6,
    min_tracking_confidence=0.6
)

detector = vision.HandLandmarker.create_from_options(options)

cap = cv2.VideoCapture(1)
timestamp = 0

base_x = None
base_y = None
prev_move_mode = False
prev_jump_state = False

SPACE_HOLD_TIME = 0.15  # seconds
space_release_time = None


def dist(a, b):
    return ((a[0] - b[0]) ** 2 + (a[1] - b[1]) ** 2) ** 0.5


def detect_hand_orientation(landmarks):
    wrist = landmarks[0]
    index_base = landmarks[5]
    pinky_base = landmarks[17]

    v1 = (index_base[0] - wrist[0], index_base[1] - wrist[1])
    v2 = (pinky_base[0] - wrist[0], pinky_base[1] - wrist[1])

    cross = v1[0] * v2[1] - v1[1] * v2[0]

    return "palm" if cross > 0 else "back"


def is_finger_curled(landmarks, tip_idx, pip_idx):
    # If fingertip is closer to wrist than the PIP joint, the finger is curled
    wrist = landmarks[0]
    d_tip = dist(wrist, landmarks[tip_idx])
    d_pip = dist(wrist, landmarks[pip_idx])
    return d_tip < d_pip


while cap.isOpened():
    success, frame = cap.read()
    if not success:
        break

    frame = cv2.flip(frame, 1)

    rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    mp_image = mp.Image(image_format=mp.ImageFormat.SRGB, data=rgb)

    timestamp += 1
    result = detector.detect_for_video(mp_image, timestamp)

    move_keys = []
    enable_control = False
    jump_triggered_this_frame = False
    ok_distance = None
    orientation = None
    curled_count = 0

    if result.hand_landmarks and result.handedness:

        for i, hand_landmarks in enumerate(result.hand_landmarks):

            handedness = result.handedness[i][0].category_name

            # Left hand only
            if handedness == "Left":

                h, w, _ = frame.shape

                landmarks = [
                    (int(lm.x * w), int(lm.y * h))
                    for lm in hand_landmarks
                ]

                # Palm side only
                orientation = detect_hand_orientation(landmarks)
                if orientation != "palm":
                    continue

                # Scale thresholds by hand size to handle camera distance changes
                hand_scale = dist(landmarks[0], landmarks[9])
                ok_threshold = hand_scale * 0.45
                move_threshold = hand_scale * 0.35

                # ===== Move ON/OFF: OK sign (thumb tip + index tip close) =====
                thumb_tip = landmarks[4]
                index_tip = landmarks[8]
                ok_distance = dist(thumb_tip, index_tip)

                if ok_distance < ok_threshold:
                    enable_control = True

                    # Set base position only at the moment OK sign is made
                    if not prev_move_mode:
                        base_x = (thumb_tip[0] + index_tip[0]) // 2
                        base_y = (thumb_tip[1] + index_tip[1]) // 2

                    prev_move_mode = True
                else:
                    enable_control = False
                    prev_move_mode = False

                # ===== WASD from displacement =====
                if base_x is not None and enable_control:
                    cur_x = (thumb_tip[0] + index_tip[0]) // 2
                    cur_y = (thumb_tip[1] + index_tip[1]) // 2

                    dx = cur_x - base_x
                    dy = cur_y - base_y

                    if dx > move_threshold:
                        move_keys.append('d')
                    elif dx < -move_threshold:
                        move_keys.append('a')

                    if dy < -move_threshold:
                        move_keys.append('w')
                    elif dy > move_threshold:
                        move_keys.append('s')

                # ===== Jump: curl middle + ring + pinky while OK sign =====
                middle_curled = is_finger_curled(landmarks, 12, 10)
                ring_curled   = is_finger_curled(landmarks, 16, 14)
                pinky_curled  = is_finger_curled(landmarks, 20, 18)
                curled_count  = sum([middle_curled, ring_curled, pinky_curled])

                # Trigger jump when 2+ fingers curl (rising edge only)
                jump_state = enable_control and curled_count >= 2
                if jump_state and not prev_jump_state:
                    jump_triggered_this_frame = True
                prev_jump_state = jump_state

                # ===== Debug drawing =====
                for (x, y) in landmarks:
                    cv2.circle(frame, (x, y), 5, (0, 255, 0), -1)

                cv2.circle(frame, thumb_tip,  10, (255, 0, 0), -1)
                cv2.circle(frame, index_tip,  10, (255, 0, 0), -1)

                if base_x is not None:
                    cv2.circle(frame, (base_x, base_y), 10, (0, 0, 255), -1)

                cv2.putText(frame, f"OK dist: {int(ok_distance)} / thr:{int(ok_threshold)}",
                            (20, 50),  cv2.FONT_HERSHEY_SIMPLEX, 0.8, (0, 255, 255), 2)
                cv2.putText(frame, f"Curled: {curled_count}/3",
                            (20, 90),  cv2.FONT_HERSHEY_SIMPLEX, 0.8, (255, 255, 0), 2)
                cv2.putText(frame, f"Orient: {orientation}",
                            (20, 130), cv2.FONT_HERSHEY_SIMPLEX, 0.8, (255, 255, 0), 2)

    # ===== Apply key input =====
    now = time.time()

    # WASD: press only needed keys, release the rest
    for k in ('w', 'a', 's', 'd'):
        if enable_control and k in move_keys:
            press_key(k)
        elif k in pressed_keys:
            keyboard.release(k)
            pressed_keys.discard(k)

    # Space: press on rising edge, auto-release after SPACE_HOLD_TIME
    if jump_triggered_this_frame:
        keyboard.press(Key.space)
        space_release_time = now + SPACE_HOLD_TIME

    if space_release_time is not None and now >= space_release_time:
        keyboard.release(Key.space)
        space_release_time = None

    label = f"Keys: {move_keys}"
    if jump_triggered_this_frame:
        label += " + SPACE"
    cv2.putText(frame, label,
                (20, 170), cv2.FONT_HERSHEY_SIMPLEX, 1.0, (255, 255, 0), 2)
    cv2.putText(frame, f"Control: {enable_control}",
                (20, 210), cv2.FONT_HERSHEY_SIMPLEX, 1.0, (0, 255, 255), 2)

    cv2.imshow("Hand Gesture WASD + Jump Controller", frame)

    if cv2.waitKey(1) & 0xFF == 27:
        break

# Cleanup
release_all()
if space_release_time is not None:
    keyboard.release(Key.space)
cap.release()
cv2.destroyAllWindows()