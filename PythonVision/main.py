import cv2
import socket
import mediapipe as mp
import numpy as np


# Camera
width, height = 1280, 720

cap = cv2.VideoCapture(0)
cap.set(3, width)
cap.set(4, height)

# Hand detection
hands_detector = mp.solutions.hands.Hands(
    static_image_mode=True,
    max_num_hands=2,
    min_detection_confidence=0.7)

# Data transfer
socket_u = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
server_address_and_port = ("127.0.0.1", 4444)

while True:
    # Getting image
    success, img = cap.read()

    # Finding hands
    flipped = np.fliplr(img)
    flippedRGB = cv2.cvtColor(flipped, cv2.COLOR_BGR2RGB)
    results = hands_detector.process(flippedRGB)

    data = []
    if results.multi_hand_landmarks:
        # Preparing data before sending it to Unity
        for landmark in results.multi_hand_landmarks[0].landmark:
            data.extend([landmark.x, 1 - landmark.y, landmark.z])

        # Sending data
        socket_u.sendto(str.encode(str(data)), server_address_and_port)
    else:
        pass
    cv2.imshow("Image", img)
    cv2.waitKey(1)


handsDetector.close()