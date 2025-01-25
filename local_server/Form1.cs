using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace smiletracker
{
    public partial class Form1 : Form
    {
        private VideoCapture capture;
        private CancellationTokenSource cts;
        private bool isStreaming = false;
        private HttpListener httpListener;
        private string streamingUrl = "http://<Your-IP>:8080/stream/";

        public Form1()
        {
            InitializeComponent();
            StartCamera();
            InitializeStreaming();
        }

        private void StartCamera()
        {
            capture = new VideoCapture(0);
            if (!capture.IsOpened())
            {
                MessageBox.Show("Can't Open WebCam");
                return;
            }

            cts = new CancellationTokenSource();
            Task.Run(() => ProcessCamera(cts.Token), cts.Token);
        }

        private async Task ProcessCamera(CancellationToken token)
        {
            using (var mat = new Mat()) // OpenCV sharp �� ��� �����ͱ���
            {
                while (!token.IsCancellationRequested)
                {
                    capture.Read(mat);
                    if (mat.Empty())
                        continue;

                    var image = BitmapConverter.ToBitmap(mat); // Bitmap ���� ��ȯ�ؾ� PictureBox�� ǥ�ð���.
                    pictureBox.Invoke(new Action(() =>
                    {
                        pictureBox.Image?.Dispose();
                        pictureBox.Image = image;
                    }));

                    Cv2.WaitKey(30); // �� 33fps
                }
            }
        }

        private void InitializeStreaming()
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(streamingUrl);
            try
            {
                httpListener.Start();
                Task.Run(() => HandleIncomingConnections());
                Debug.WriteLine("��Ʈ���� ���� ���۵�.");
            }
            catch (HttpListenerException ex)
            {
                MessageBox.Show($"��Ʈ���� ������ ������ �� �����ϴ�: {ex.Message}\n������ �������� ������ �ּ���.");
                Debug.WriteLine($"��Ʈ���� ���� ���� ����: {ex.Message}");
            }
        }

        private async Task HandleIncomingConnections()
        {
            while (httpListener.IsListening)
            {
                try
                {
                    var context = await httpListener.GetContextAsync();
                    if (context.Request.Url.AbsolutePath == "/stream/")
                    {
                        Debug.WriteLine("Ŭ���̾�Ʈ ���� ������.");
                        context.Response.ContentType = "multipart/x-mixed-replace; boundary=--myboundary";
                        var boundary = "--myboundary\r\n";
                        var boundaryBytes = System.Text.Encoding.ASCII.GetBytes(boundary);
                        var jpegHeader = "Content-Type: image/jpeg\r\nContent-Length: ";

                        while (isStreaming && httpListener.IsListening)
                        {
                            using (var mat = new Mat())
                            {
                                capture.Read(mat);
                                if (mat.Empty())
                                    continue;

                                using (var ms = new MemoryStream())
                                {
                                    mat.ToBitmap().Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    var imageBytes = ms.ToArray();

                                    var header = $"{jpegHeader}{imageBytes.Length}\r\n\r\n";
                                    var headerBytes = System.Text.Encoding.ASCII.GetBytes(header);

                                    await context.Response.OutputStream.WriteAsync(headerBytes, 0, headerBytes.Length);
                                    await context.Response.OutputStream.WriteAsync(imageBytes, 0, imageBytes.Length);
                                    await context.Response.OutputStream.WriteAsync(System.Text.Encoding.ASCII.GetBytes("\r\n"), 0, 2);
                                    await context.Response.OutputStream.WriteAsync(boundaryBytes, 0, boundaryBytes.Length);

                                    Debug.WriteLine($"������ ���۵�: {imageBytes.Length} ����Ʈ");
                                }
                            }

                            await Task.Delay(100); // �� 10fps
                        }

                        context.Response.OutputStream.Close();
                        Debug.WriteLine("Ŭ���̾�Ʈ ��Ʈ���� �����.");
                    }
                    else
                    {
                        context.Response.StatusCode = 404;
                        context.Response.Close();
                        Debug.WriteLine($"�߸��� ��û: {context.Request.Url.AbsolutePath}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"��Ʈ���� ó�� �� ���� �߻�: {ex.Message}");
                    Debug.WriteLine($"��Ʈ���� ó�� �� ���� �߻�: {ex.Message}");
                }
            }
        }

        private void recordButton_Click(object sender, EventArgs e)
        {
            if (!isStreaming)
            {
                isStreaming = true;
                recordButton.Text = "��Ʈ���� ����";
                Debug.WriteLine("��Ʈ���� ���۵�.");
            }
            else
            {
                isStreaming = false;
                recordButton.Text = "��ȭ";
                Debug.WriteLine("��Ʈ���� ������.");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            cts?.Cancel();
            capture?.Release();
            httpListener?.Stop();
            base.OnFormClosing(e);
            Debug.WriteLine("���ø����̼� �����.");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
