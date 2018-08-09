﻿#region License of Sample

/*
CameraControlTool - is a complicated sample of CameraControl usage
Copyright (C) 2013
https://github.com/free5lot/Camera_Net

While the Camera_Net library is covered by LGPL, 
this sample is released as PUBLIC DOMAIN.
So, you can use code from this sample in your 
free or proprietary project without any limitations.

It is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
*/

#endregion

namespace CameraControlTool
{
    #region Using directives

    using System;
    using System.Windows.Forms;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Runtime.InteropServices.ComTypes;
    using gfoidl.Imaging;
    using Camera_NET;

    using DirectShowLib;

    #endregion

    public partial class FormCameraControlTool : Form
    {
        #region Vars

        // Camera object
        //private Camera _Camera;

        // Rect selection with mouse
        private NormalizedRect _MouseSelectionRect = new NormalizedRect(0, 0, 0, 0);
        private bool _bDrawMouseSelection = false;

        // Zooming
        private bool _bZoomed = false;
        private double _fZoomValue = 1.0;

        // Camera choice
        private CameraChoice _CameraChoice = new CameraChoice();

        #endregion

        #region Winforms stuff

        // Constructor
        public FormCameraControlTool()
        {
            InitializeComponent();
            //Added by Anand Start
            this.ScreenShot = new Bitmap(500, 500);
            //cameraControl.MixerEnabled = !cameraControl.MixerEnabled;
            //End
            //SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
        }

        // On load of Form
        private void FormCameraControlTool_Load(object sender, EventArgs e)
        {
            // Fill camera list combobox with available cameras
            FillCameraList();

            // Select the first one
            if (comboBoxCameraList.Items.Count > 0)
            {
                comboBoxCameraList.SelectedIndex = 0;
            }

            // Fill camera list combobox with available resolutions
            FillResolutionList();
        }

        // On close of Form
        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Close camera
            cameraControl.CloseCamera();
        }

        // Update buttons of GUI based on Camera state
        private void UpdateGUIButtons()
        {
            buttonCrossbarSettings.Enabled = (cameraControl.CrossbarAvailable);
        }

        #endregion

        #region Camera creation and destroy

        // Set current camera to camera_device
        private void SetCamera(IMoniker camera_moniker, Resolution resolution)
        {
            try
            {
                // NOTE: You can debug with DirectShow logging:
                //cameraControl.DirectShowLogFilepath = @"C:\YOUR\LOG\PATH.txt";

                // Makes all magic with camera and DirectShow graph
                cameraControl.SetCamera(camera_moniker, resolution);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, @"Error while running camera");
            }

            if (!cameraControl.CameraCreated)
                return;

            // If you are using Direct3D surface overlay
            // (see documentation about rebuild of library for it)
            //cameraControl.UseGDI = false;

            cameraControl.MixerEnabled = true;

            cameraControl.OutputVideoSizeChanged += Camera_OutputVideoSizeChanged;

            UpdateCameraBitmap();            


            // gui update
            UpdateGUIButtons();
        }

        #endregion

        #region Functions Buttons
        public Bitmap ScreenShot { get; set; }
        
        private void buttonMixerOnOff_Click(object sender, EventArgs e)
        {
            cameraControl.MixerEnabled = !cameraControl.MixerEnabled;
        }
        //Added by Anand Start
        public Bitmap FixedSize(Bitmap image, int Width, int Height, bool needToFill)
        {
            #region много арифметики
            int sourceWidth = image.Width;
            int sourceHeight = image.Height;
            int sourceX = 0;
            int sourceY = 0;
            double destX = 0;
            double destY = 0;

            double nScale = 0;
            double nScaleW = 0;
            double nScaleH = 0;

            nScaleW = ((double)Width / (double)sourceWidth);
            nScaleH = ((double)Height / (double)sourceHeight);
            if (!needToFill)
            {
                nScale = Math.Min(nScaleH, nScaleW);
            }
            else
            {
                nScale = Math.Max(nScaleH, nScaleW);
                destY = (Height - sourceHeight * nScale) / 2;
                destX = (Width - sourceWidth * nScale) / 2;
            }

            if (nScale > 1)
                nScale = 1;

            int destWidth = (int)Math.Round(sourceWidth * nScale);
            int destHeight = (int)Math.Round(sourceHeight * nScale);
            #endregion

            System.Drawing.Bitmap bmPhoto = null;
            try
            {
                bmPhoto = new System.Drawing.Bitmap(destWidth + (int)Math.Round(2 * destX), destHeight + (int)Math.Round(2 * destY));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("destWidth:{0}, destX:{1}, destHeight:{2}, desxtY:{3}, Width:{4}, Height:{5}",
                    destWidth, destX, destHeight, destY, Width, Height), ex);
            }
            using (System.Drawing.Graphics grPhoto = System.Drawing.Graphics.FromImage(bmPhoto))
            {
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic; // _interpolationMode;
                grPhoto.CompositingQuality = CompositingQuality.HighQuality; // _compositingQuality;
                grPhoto.SmoothingMode = SmoothingMode.HighQuality; ;// _smoothingMode;

                Rectangle to = new System.Drawing.Rectangle((int)Math.Round(destX), (int)Math.Round(destY), destWidth, destHeight);
                Rectangle from = new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight);
                //Console.WriteLine("From: " + from.ToString());
                //Console.WriteLine("To: " + to.ToString());
                grPhoto.DrawImage(image, to, from, System.Drawing.GraphicsUnit.Pixel);

                return bmPhoto as Bitmap;
            }
        }
        //End
        private void buttonSnapshotOutputFrame_Click(object sender, EventArgs e)
        {
            if (!cameraControl.CameraCreated)
                return;

            Bitmap bitmap = cameraControl.SnapshotOutputImage();
            Bitmap FinalImage = FixedSize(bitmap, 118, 150, true);
            //Added By Anand
            //Bitmap newImage = new Bitmap(118, 150);
            //using (Graphics gr = Graphics.FromImage(newImage))
            //{
            //    gr.SmoothingMode = SmoothingMode.HighQuality;
            //    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //    gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
            //    gr.DrawImage(bitmap, new Rectangle(0, 0, 118, 150));
            //}

            ScreenShot = FinalImage;
            if (bitmap == null)
                return;

            pictureBoxScreenshot.Image = FinalImage;
            pictureBoxScreenshot.Update();
        }

        private void buttonSnapshotNextSourceFrame_Click(object sender, EventArgs e)
        {
            if (!cameraControl.CameraCreated)
                return;

            Bitmap bitmap = null;
            try
            {
                bitmap = cameraControl.SnapshotSourceImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Error while getting a snapshot");
            }

            if (bitmap == null)
                return;

            pictureBoxScreenshot.Image = bitmap;
            pictureBoxScreenshot.Update();
        }

        private void buttonClearSnapshotFrame_Click(object sender, EventArgs e)
        {
            pictureBoxScreenshot.Image = null;
            pictureBoxScreenshot.Update();
        }

        // Example of crossbar usage
        private void buttonCrossbarSettings_Click(object sender, EventArgs e)
        {
            if (cameraControl.CameraCreated)
            {
                cameraControl.DisplayPropertyPage_Crossbar(this.Handle);
            }
        }

        // Example of TVMode usage
        private void buttonTVMode_Click(object sender, EventArgs e)
        {
            if (cameraControl.CameraCreated)
            {
                MessageBox.Show(cameraControl.GetTVMode().ToString());
            }            
        }

        private void buttonCameraSettings_Click(object sender, EventArgs e)
        {
            if (cameraControl.CameraCreated)
            {
                Camera.DisplayPropertyPage_Device(cameraControl.Moniker, this.Handle);
            }
        }

        #endregion

        #region Camera event handlers

        // Event handler for OutputVideoSizeChanged event
        private void Camera_OutputVideoSizeChanged(object sender, EventArgs e)
        {
            // Update camera's bitmap (new size needed)
            UpdateCameraBitmap();

            // Place Zoom button in correct place on form
            UpdateUnzoomButton();
        }

        #endregion

        #region Overlay bitmaps stuff

        // Generate bitmap for overlay
        private void UpdateCameraBitmap()
        {
            if (!cameraControl.MixerEnabled)
                return;

            cameraControl.OverlayBitmap = GenerateColorKeyBitmap(false);
            //Added by Anand Start
            cameraControl.MixerEnabled = !cameraControl.MixerEnabled;
            //End
            #region D3D bitmap mixer
            //if (cameraControl.UseGDI)
            //{
            //    cameraControl.OverlayBitmap = GenerateColorKeyBitmap(false);
            //}
            //else
            //{
            //    cameraControl.OverlayBitmap = GenerateAlphaBitmap();
            //}
            #endregion
        }

        // NOTE: This function is an example of overlay bitmap usage
        // Create bitmap with selection rect, text or any other overlay
        private Bitmap GenerateColorKeyBitmap(bool useAntiAlias)
        {
            int w = cameraControl.OutputVideoSize.Width;
            int h = cameraControl.OutputVideoSize.Height;

            if (w <= 0 || h <= 0)
                return null;

            // Create RGB bitmap
            Bitmap bmp = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(bmp);

            // configure antialiased drawing or not
            if (useAntiAlias)
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            else
            {
                g.SmoothingMode = SmoothingMode.None;
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            }

            // Clear the bitmap with the color key
            g.Clear(cameraControl.GDIColorKey);

            // Draw selection rect --------------------------
            if (_bDrawMouseSelection && IsMouseSelectionRectCorrect())
            {
                Color color_of_pen = Color.Gray;
                if (IsMouseSelectionRectCorrectAndGood())
                {
                    color_of_pen = Color.Green;
                }

                Pen pen = new Pen(color_of_pen, 2.0f);

                Rectangle rect = new Rectangle(
                        (int)(_MouseSelectionRect.left * w),
                        (int)(_MouseSelectionRect.top * h),
                        (int)((_MouseSelectionRect.right - _MouseSelectionRect.left) * w),
                        (int)((_MouseSelectionRect.bottom - _MouseSelectionRect.top) * h)
                    );



                g.DrawLine(pen, rect.Left - 5, rect.Top, rect.Right + 5, rect.Top);
                g.DrawLine(pen, rect.Left - 5, rect.Bottom, rect.Right + 5, rect.Bottom);
                g.DrawLine(pen, rect.Left, rect.Top - 5, rect.Left, rect.Bottom + 5);
                g.DrawLine(pen, rect.Right, rect.Top - 5, rect.Right, rect.Bottom + 5);

                pen.Dispose();

            }

            // Draw zoom text if needed
            if (_bZoomed)
            {
                Font font = new Font("Tahoma", 16);
                Brush textColorKeyed = new SolidBrush(Color.DarkBlue);

                g.DrawString("Zoom: " + Math.Round(_fZoomValue, 1).ToString("0.0") + "x", font, textColorKeyed, 4, 4);

                font.Dispose();
                textColorKeyed.Dispose();
            }

            // Draw text logo for example
            {
                Font font = new Font("Tahoma", 16);
                Brush textColorKeyed = new SolidBrush(Color.Yellow);

                g.DrawString("Sample project for Camera_NET component", font, textColorKeyed, 4, h - 30);

            }


            // dispose Graphics
            g.Dispose();

            // return the bitmap
            return bmp;
        }

        #region D3D bitmap mixer
        //public Bitmap GenerateAlphaBitmap()
        //{
        //    // Alpha values
        //    int alpha50 = (int)(255 * 0.50f); // 50% opacity

        //    // Some drawing tools needed later
        //    Pen blackBorder = new Pen(Color.Black, 2.0f);
        //    Brush red50 = new SolidBrush(Color.FromArgb(alpha50, Color.Red));
        //    Font font = new Font("Tahoma", 16);

        //    int w = _Camera.OutputVideoSize.Width;
        //    int h = _Camera.OutputVideoSize.Height;

        //    // Create a ARGB bitmap
        //    Bitmap bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
        //    Graphics g = Graphics.FromImage(bmp);

        //    // Do antialiased drawings
        //    g.SmoothingMode = SmoothingMode.AntiAlias;
        //    g.TextRenderingHint = TextRenderingHint.AntiAlias;

        //    // Clear the bitmap with complete transparency
        //    g.Clear(Color.Transparent);

        //    // Draw a green circle with black border in the middle
        //    //g.FillEllipse(green, 320 * w / 640, 240 * h / 480, 155 * w / 640, 155 * h / 480);
        //    g.FillEllipse(red50, w / 2 - 70, h / 2 - 70, 140, 140);
        //    g.DrawEllipse(blackBorder, w / 2 - 70, h / 2 - 70, 140, 140);


        //    // Release GDI+ objects
        //    blackBorder.Dispose();
        //    red50.Dispose();
        //    g.Dispose();

        //    // return the bitmap
        //    return bmp;
        //}
        #endregion

        // Swritch GDI/D3D
        //private void button3_Click(object sender, EventArgs e)
        //{
        //    cameraControl.MixerEnabled = false;

        //    cameraControl.UseGDI = !cameraControl.UseGDI;
        //    UpdateCameraBitmap();

        //    if (!cameraControl.MixerEnabled)
        //        cameraControl.MixerEnabled = true;
        //}

        #endregion

        #region Mouse selection stuff

        
        
        private void cameraControl_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void cameraControl_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void cameraControl_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private bool IsMouseSelectionRectCorrect()
        {
            if (Math.Abs(_MouseSelectionRect.right - _MouseSelectionRect.left) < float.Epsilon*10 ||
                Math.Abs(_MouseSelectionRect.bottom - _MouseSelectionRect.top) < float.Epsilon*10)
            {
                return false;
            }

            if (_MouseSelectionRect.left >= _MouseSelectionRect.right ||
                _MouseSelectionRect.top >= _MouseSelectionRect.bottom)
            {
                return false;
            }

            if (_MouseSelectionRect.left < 0 ||
                _MouseSelectionRect.top < 0 ||
                _MouseSelectionRect.right > 1.0 ||
                _MouseSelectionRect.bottom > 1.0)
            {
                return false;
            }
            return true;
        }

        private bool IsMouseSelectionRectCorrectAndGood()
        {
            if (! IsMouseSelectionRectCorrect())
            {
                return false;

            }

            // Zoom
            if (Math.Abs(_MouseSelectionRect.right - _MouseSelectionRect.left) < 0.1f ||
                Math.Abs(_MouseSelectionRect.bottom - _MouseSelectionRect.top) < 0.1f)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Zooming stuff

        // Unzoom on video double-click
        private void cameraControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            UnzoomCamera();
        }

        // Unzoom with unzoom button
        private void buttonUnZoom_Click(object sender, EventArgs e)
        {
            UnzoomCamera();
        }


        // Unzoom camera and update gui
        private void UnzoomCamera()
        {
            cameraControl.ZoomToRect(new Rectangle(0, 0, cameraControl.Resolution.Width, cameraControl.Resolution.Height));

            _bZoomed = false;
            _fZoomValue = 1.0;

            // gui updates
            UpdateCameraBitmap();
            UpdateUnzoomButton();

            _bDrawMouseSelection = false;
        }


        // Place Zoom button in correct place on form
        private void UpdateUnzoomButton()
        {
            if (_bZoomed)
            {
                buttonUnZoom.Left = cameraControl.Left + (cameraControl.ClientRectangle.Width - cameraControl.OutputVideoSize.Width) / 2 + 4;
                buttonUnZoom.Top = cameraControl.Top + (cameraControl.ClientRectangle.Height - cameraControl.OutputVideoSize.Height) / 2 + 40;
                buttonUnZoom.Visible = true;
            }
            else
            {
                buttonUnZoom.Visible = false;
            }
        }

        #endregion

        #region Camera and resolution selection

        private void comboBoxCameraList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCameraList.SelectedIndex < 0)
            {
                cameraControl.CloseCamera();
            }
            else
            {
                // Set camera
                SetCamera(_CameraChoice.Devices[ comboBoxCameraList.SelectedIndex ].Mon, null);
            }

            FillResolutionList();
        }

        private void comboBoxResolutionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cameraControl.CameraCreated)
                return;

            int comboBoxResolutionIndex = comboBoxResolutionList.SelectedIndex;
            if (comboBoxResolutionIndex < 0)
            {
                return;
            }
            ResolutionList resolutions = Camera.GetResolutionList(cameraControl.Moniker);

            if ( resolutions == null )
                return; 

            if ( comboBoxResolutionIndex >= resolutions.Count )
                return; // throw

            if (0 == resolutions[comboBoxResolutionIndex].CompareTo(cameraControl.Resolution))
            {
                // this resolution is already selected
                return;
            }

            // Recreate camera
            SetCamera(cameraControl.Moniker, resolutions[comboBoxResolutionIndex]);

        }

        private void FillResolutionList()
        {
            comboBoxResolutionList.Items.Clear();

            if (!cameraControl.CameraCreated)
                return;

            ResolutionList resolutions = Camera.GetResolutionList(cameraControl.Moniker);

            if (resolutions == null)
                return;


            int index_to_select = -1;

            for (int index = 0; index < resolutions.Count; index++)
            {
                comboBoxResolutionList.Items.Add(resolutions[index].ToString());

                if (resolutions[index].CompareTo(cameraControl.Resolution) == 0)
                {
                    index_to_select = index;
                }
            }

            // select current resolution
            if (index_to_select >= 0)
            {
                comboBoxResolutionList.SelectedIndex = index_to_select;
            }
        }

        private void FillCameraList()
        {
            comboBoxCameraList.Items.Clear();

            _CameraChoice.UpdateDeviceList();

            foreach (var camera_device in _CameraChoice.Devices)
            {
                comboBoxCameraList.Items.Add(camera_device.Name);
            }
        }

        #endregion

        private void buttonPinOutputSettings_Click(object sender, EventArgs e)
        {
            if (cameraControl.CameraCreated)
            {
                cameraControl.DisplayPropertyPage_SourcePinOutput(this.Handle);
            }
        }
        
    }
}
