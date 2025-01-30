import cv2

# 노트북의 IP 주소를 입력하세요. 예: "192.168.1.100"
stream_url = "http://192.168.35.125:8080/stream/"

cap = cv2.VideoCapture(stream_url)

if not cap.isOpened():
    print("스트림을 열 수 없습니다.")
    exit()

while True:
    ret, frame = cap.read() # ret : 프로그램을 성공적으로 읽었는지 여부, frame 은 읽은 프레임
    if not ret:
        print("프레임을 읽을 수 없습니다.")
        break

    cv2.imshow('Stream', frame) # 읽은 프레임 표시 

    if cv2.waitKey(1) & 0xFF == ord('q'): # 키 입력을 기다린다. q입력시 종료 
        break

cap.release()
cv2.destroyAllWindows()
