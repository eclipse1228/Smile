# Smile Tracker

Smile Tracker는 C#과 Python으로 구성된 프로젝트로, 사용자의 미소를 추적하여 스트리밍 데이터를 분석합니다. 이 프로젝트는 다음 두 가지 주요 컴포넌트로 구성됩니다:
1. **C# Streaming Server**: 웹캠에서 실시간 비디오 스트림을 제공합니다.
2. **Python Streaming Client**: 제공된 스트림에 연결하여 데이터를 수신하고 분석합니다.

---

## 프로젝트 구조

```
smile_tracker/
│
├── python_project/       # Python 클라이언트
│   ├── stream_receiver.py
│   ├── requirements.txt
│   ├── Dockerfile
│   └── README.md
│
├── csharp_project/       # C# 서버
│   ├── smile_tracker_console.sln
│   ├── smile_tracker_console.csproj
│   ├── Program.cs
│   ├── Dockerfile
│   └── README.md
│
├── README.md             # 프로젝트 가이드
└── .gitignore            # Git 무시 규칙
```

---

## 요구 사항

Smile Tracker를 실행하기 위해 다음 도구가 필요합니다:

### 공통 요구 사항
- **Docker**: 

### Python 클라이언트
- **Python 3.10+**
- OpenCV 및 기타 종속 라이브러리

### C# 서버
- **.NET 7.0 SDK 이상**
- Visual Studio 2022 (권장)

---

## 설치 및 실행

### 1. C# 서버 설정 및 실행

#### 로컬 실행

1. **필수 라이브러리 설치**:
   C# 프로젝트에 `OpenCvSharp4` 및 `OpenCvSharp4.runtime.win` 패키지가 설치되어 있어야 합니다. 설치되지 않은 경우 다음 명령어를 실행하세요:
   ```bash
   dotnet add package OpenCvSharp4 --version 4.8.0.20230824
   dotnet add package OpenCvSharp4.runtime.win --version 4.8.0.20230824
   ```

2. **Visual Studio에서 실행**:
   - `smile_tracker_console.sln` 파일을 Visual Studio에서 열고 실행합니다.

3. **방화벽 설정**:
   스트리밍 서버는 **8080 포트**를 사용합니다. 이를 허용하도록 Windows 방화벽 설정을 수정하세요:
   ```bash
   netsh http add urlacl url=http://+:8080/ user=Everyone
   ```

4. **스트리밍 서버 실행 확인**:
   브라우저에서 [http://localhost:8080/stream/](http://localhost:8080/stream/)로 접속하여 스트림이 출력되는지 확인하세요.

#### Docker 실행

1. **이미지 빌드**:
   ```bash
   docker build -t smile_tracker_server ./csharp_project
   ```

2. **컨테이너 실행**:
   ```bash
   docker run -d -p 8080:8080 --name smile_tracker_server smile_tracker_server
   ```

---

### 2. Python 클라이언트 설정 및 실행

#### 로컬 실행

1. **필수 라이브러리 설치**:
   Python 클라이언트가 필요로 하는 라이브러리를 설치합니다:
   ```bash
   pip install -r python_project/requirements.txt
   ```

2. **Python 스크립트 실행**:
   Python 스크립트를 실행하여 스트리밍 서버에 연결합니다:
   ```bash
   python python_project/stream_receiver.py
   ```

3. **스트림 확인**:
   스크립트가 실행되면 스트리밍 서버에서 제공하는 비디오 스트림을 수신하여 출력합니다.

#### Docker 실행

1. **이미지 빌드**:
   ```bash
   docker build -t smile_tracker_client ./python_project
   ```

2. **컨테이너 실행**:
   ```bash
   docker run -it --rm smile_tracker_client
   ```

---

### 3. 방화벽 포트 설정

스트리밍 서버와 클라이언트가 통신하려면 **8080 포트**가 열려 있어야 합니다.

#### 포트 열기:
Windows 방화벽에서 8080 포트를 허용합니다:
```bash
netsh advfirewall firewall add rule name="Smile Tracker Server" dir=in action=allow protocol=TCP localport=8080
```

#### 포트 닫기:
더 이상 사용하지 않는 경우 8080 포트를 닫을 수 있습니다:
```bash
netsh advfirewall firewall delete rule name="Smile Tracker Server"
```

---

### 4. Docker Compose 사용 (선택 사항)

두 프로젝트를 함께 실행하려면 `docker-compose`를 사용할 수 있습니다.

#### `docker-compose.yml` 예시:
```yaml
version: '3.8'

services:
  csharp_server:
    build:
      context: ./csharp_project
    ports:
      - "8080:8080"

  python_client:
    build:
      context: ./python_project
    depends_on:
      - csharp_server
```

#### 실행 명령어:
```bash
docker-compose up --build
```

---

## 문제 해결

1. **스트림이 연결되지 않을 경우**:
   - 스트리밍 서버가 실행 중인지 확인하세요.
   - 방화벽 설정이 올바른지 확인하세요.

2. **Docker 컨테이너 내부 디버깅**:
   컨테이너 내부에서 종속 라이브러리가 설치되었는지 확인하세요:
   ```bash
   docker exec -it <container_name> /bin/bash
   ```

3. **OpenCV 관련 오류**:
   OpenCV 네이티브 라이브러리가 설치되었는지 확인하세요:
   ```bash
   apt-get install -y libglib2.0-0 libopencv-dev
   ```

---

## 참고 자료

- **OpenCV Python**: [https://pypi.org/project/opencv-python/](https://pypi.org/project/opencv-python/)
- **.NET 7.0 SDK**: [https://dotnet.microsoft.com/download/dotnet/7.0](https://dotnet.microsoft.com/download/dotnet/7.0)
- **Docker Documentation**: [https://docs.docker.com/](https://docs.docker.com/)

---

## 프로젝트 기여

Smile Tracker 프로젝트에 기여하고 싶다면 다음 절차를 따라주세요:

1. 이 레포지토리를 포크합니다.
2. 새 브랜치를 생성합니다:
   ```bash
   git checkout -b feature/your-feature
   ```
3. 변경사항을 커밋합니다:
   ```bash
   git commit -m "Add your feature"
   ```
4. 브랜치를 푸시합니다:
   ```bash
   git push origin feature/your-feature
   ```
5. Pull Request를 생성합니다.

---

이 가이드를 사용하면 Smile Tracker 프로젝트를 쉽게 설정하고 실행할 수 있습니다. 질문이 있으면 언제든지 문의해 주세요! 😊