import cv2
import cv2.data


def detect_smile():
    # face_cascade_path = 'C:\smile_track\raspberry_server\haarcascade_frontalface_default.xml'
    # smile_cascade_path = 'C:\smile_track\raspberry_server\haarcascade_smile.xml'
    face_cascade = cv2.CascadeClassifier(cv2.data.haarcascades + "haarcascade_frontalface_default.xml")
    smile_cascade = cv2.CascadeClassifier(cv2.data.haarcascades + "haarcascade_smile.xml")

     # Haar Cascade 로드

    # face_cascade = cv2.CascadeClassifier(face_cascade_path)
    # smile_cascade = cv2.CascadeClassifier(smile_cascade_path)

    # 웹캠 초기화 (기본 카메라: 0)
    cap = cv2.VideoCapture(0)

    if not cap.isOpened():
        print("웹캠을 열 수 없습니다.")
        return

    while True:
        # 프레임 읽기
        ret, frame = cap.read()
        if not ret:
            print("프레임을 읽을 수 없습니다.")
            break

        # 프레임을 그레이스케일로 변환
        gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

        # 얼굴 검출
        faces = face_cascade.detectMultiScale(
            gray,
            scaleFactor=1.3,
            minNeighbors=5,
            minSize=(60, 60)
        )

        for (x, y, w, h) in faces:
            # 얼굴 영역에 사각형 그리기
            cv2.rectangle(frame, (x, y), (x + w, y + h), (255, 0, 0), 2)

            # 얼굴 영역 ROI 추출
            roi_gray = gray[y:y + h, x:x + w]
            roi_color = frame[y:y + h, x:x + w]

            # 미소 검출
            smiles = smile_cascade.detectMultiScale(
                roi_gray,
                scaleFactor=1.3,  # (1.1~1.4)이미지 피라미드 스케일링 계수 (이미지 크기를 얼마나 줄여가며 검출할지 결정 (작을수록 정확))
                minNeighbors=10,  # (5~10)검출된 사각형 주변에 최소 몇개 이웃이 있어야 검출된다고 '판단'할지
                minSize=(25, 25)  # (20~40) 검출할 얼굴 최소 크기. 
            )

            for (sx, sy, sw, sh) in smiles:
                # 미소 영역에 사각형 그리기
                cv2.rectangle(roi_color, (sx, sy), (sx + sw, sy + sh), (0, 255, 0), 2)
                cv2.putText(frame, 'Smiling', (x, y - 10), cv2.FONT_HERSHEY_SIMPLEX, 
                            0.9, (0, 255, 0), 2)

        # 결과 프레임 출력
        cv2.imshow('Smile Detector', frame)

        # 'q' 키를 누르면 종료
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

    # 자원 해제
    cap.release()
    cv2.destroyAllWindows()

if __name__ == "__main__":
    detect_smile()