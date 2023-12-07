using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project5
{
    public partial class Form1 : Form
    {
        private bool drawing = false;
        private Point startPoint;
        private Point endPoint;
        private Image loadedImage;
        private string currentImagePath;
        private string fileNameWithoutExtension;
        private string fileExtension;


        public Form1()
        {
            InitializeComponent();
        }

        /*private void btn_load_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "이미지 파일|*.png;*.jpg;*.jpeg;*.gif;*.bmp|모든 파일|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // 파일 전체 경로
                    string filePath = openFileDialog.FileName;

                    // 파일 이름만 추출
                    fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

                    // 확장자만 추출
                    fileExtension = Path.GetExtension(filePath);

                    // 이미지를 PictureBox에 표시
                    loadedImage = Image.FromFile(filePath);
                    pictureBox1.Image = loadedImage;

                    // 열린 이미지의 경로 저장
                    currentImagePath = filePath;

                    openFileDialog.Dispose();
                }
            }
        }*/

        private void btn_load_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "이미지 파일|*.png;*.jpg;*.jpeg;*.gif;*.bmp|모든 파일|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    // BinaryReader를 사용하여 이미지 파일의 바이너리 데이터 읽기
                    byte[] imageData = ReadImageBinaryData(filePath);

                    if (imageData != null)
                    {
                        // MemoryStream을 사용하여 바이너리 데이터를 Image 객체로 변환
                        Image image = ConvertByteArrayToImage(imageData);

                        // PictureBox에 이미지 표시
                        pictureBox1.Image = image;

                        // 열린 이미지의 경로 저장
                        currentImagePath = filePath;
                    }
                    else
                    {
                        MessageBox.Show("이미지 파일을 읽는 중에 오류가 발생했습니다.");
                    }
                }
            }
        }

        private byte[] ReadImageBinaryData(string filePath)
        {
            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                {
                    return reader.ReadBytes((int)reader.BaseStream.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"이미지 파일을 읽는 중에 오류가 발생했습니다. 오류 메시지: {ex.Message}");
                return null;
            }
        }

        private Image ConvertByteArrayToImage(byte[] imageData)
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream(imageData))
                {
                    return Image.FromStream(memoryStream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"이미지를 Image 객체로 변환하는 중에 오류가 발생했습니다. 오류 메시지: {ex.Message}");
                return null;
            }
        }

        /*private void btn_save_Click(object sender, EventArgs e)
        {
            if (loadedImage != null)
            {
                if (!string.IsNullOrEmpty(currentImagePath))
                {
                    // 현재 이미지 경로가 설정되어 있으면 그 경로에 직접 저장
                    pictureBox1.Image.Save(currentImagePath);
                    MessageBox.Show("이미지가 성공적으로 저장되었습니다.");
                }
                else
                {
                    MessageBox.Show("이미지를 저장할 파일이 지정되지 않았습니다. 먼저 이미지를 열어주세요.");
                }
            }
            else
            {
                MessageBox.Show("이미지가 로드되지 않았습니다. 먼저 이미지를 열어주세요.");
            }
        }*/

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentImagePath))
            {
                // 현재 이미지 경로가 설정되어 있으면 그 경로에 직접 저장
                pictureBox1.Image.Save(currentImagePath);
                MessageBox.Show("이미지가 성공적으로 저장되었습니다.");
            }
            else
            {
                MessageBox.Show("이미지를 저장할 파일이 지정되지 않았습니다. 먼저 이미지를 열어주세요.");
            }
        }

        private void btn_new_save_Click(object sender, EventArgs e)
        {
            if (loadedImage != null)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PNG 파일|*.png|JPEG 파일|*.jpg;*.jpeg|BMP 파일|*.bmp|GIF 파일|*.gif|모든 파일|*.*";

                    // 초기 파일 이름 설정
                    saveFileDialog.FileName = fileNameWithoutExtension + fileExtension;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = saveFileDialog.FileName;

                        // PictureBox의 이미지를 파일로 저장
                        pictureBox1.Image.Save(filePath);

                        MessageBox.Show("이미지가 성공적으로 저장되었습니다.");
                    }
                }
            }
            else
            {
                MessageBox.Show("이미지가 로드되지 않았습니다. 먼저 이미지를 열어주세요.");
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            drawing = true;
            startPoint = e.Location;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                endPoint = e.Location;

                // PictureBox에 선을 그림
                using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                {
                    using (Pen pen = new Pen(Color.Black, 2))
                    {
                        g.DrawLine(pen, startPoint, endPoint);
                    }
                }

                startPoint = endPoint;
                pictureBox1.Invalidate(); // PictureBox를 다시 그리도록 갱신
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            drawing = false;
        }
    }
}
