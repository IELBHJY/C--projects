#include<highgui.h>  
#include<cv.h>  
#include<iostream>  
#include<fstream>  
using namespace cv;
using namespace std;  
void save_result(CvMat*cam_rotation_all, CvMat*cam_translation_vector_all,  
    CvMat*cam_intrinsic_matrix, CvMat*cam_distortion_coeffs,char*pathc,int sucesses);  
int main()  
{  
  CvMat*cam_object_points2;  
  CvMat*cam_image_points2;  
  int cam_board_n;  
  int successes = 0;  
    int img_num, cam_board_w, cam_board_h,cam_Dx,cam_Dy;  
    cout << "输入的图像的组数\n";  
    cin >> img_num;  
    cout << "输入**真实**棋盘格的##横轴##方向的角点个数\n";  
    cin >> cam_board_w;  
    cout << "输入**真实**棋盘格的##纵轴##方向的角点个数\n";  
    cin >> cam_board_h;  
    cout << "输入**真实**棋盘格的##横轴##方向的长度\n";  
    cin >> cam_Dx;  
    cout << "输入**真实**棋盘格的##纵轴##方向的长度\n";  
    cin >> cam_Dy;  
    cam_board_n = cam_board_w*cam_board_h;  
    //camera init  
    CvSize cam_board_sz = cvSize(cam_board_w, cam_board_h);  
    CvMat*cam_image_points = cvCreateMat(cam_board_n*(img_num), 2, CV_32FC1);  
    CvMat*cam_object_points = cvCreateMat(cam_board_n*(img_num), 3, CV_32FC1);  
    CvMat*cam_point_counts = cvCreateMat((img_num), 1, CV_32SC1);  
    CvPoint2D32f*cam_corners = new CvPoint2D32f[cam_board_n];  
    int cam_corner_count;  
    int cam_step;  
    CvMat*cam_intrinsic_matrix = cvCreateMat(3, 3, CV_32FC1);  
    CvMat*cam_distortion_coeffs = cvCreateMat(5, 1, CV_32FC1);  
    CvSize cam_image_sz;  
	//CvCapture *capture=cvCreateCameraCapture(0);
    //cvNamedWindow("Calibration", 1);  
    //get image size 
	//输入图片路径
    IplImage *cam_image_temp = cvLoadImage("E:/复杂机电/单目标定图片/000000.png");  
    cam_image_sz = cvGetSize(cam_image_temp); 
    char failurebuf[20] = { 0 };  
	//--------------------------------
    fstream cam_data;  
    cam_data.open("E:/复杂机电/单目标定图片\\cam_corners.txt", ofstream::out);  
    fstream cam_object_data;  
    cam_object_data.open("E:/复杂机电/单目标定图片\\cam_object_data.txt", ofstream::out);  
    //process the prj image so that we can easy find cornner  
    int k=0;
	while(k<10)
	{
	    int ii=k;
        char cambuf[20] = { 0 };  
        sprintf(cambuf, "E:/复杂机电/单目标定图片\\00000%d.png", ii); //读取图片 
        IplImage *cam_image = cvLoadImage(cambuf, 0);  
        //extract cam cornner  
        int cam_found = cvFindChessboardCorners(cam_image, cam_board_sz, cam_corners, &cam_corner_count,  
             CV_CALIB_CB_ADAPTIVE_THRESH | CV_CALIB_CB_FILTER_QUADS);  
        cvFindCornerSubPix(cam_image, cam_corners, cam_corner_count,  
            cvSize(11, 11), cvSize(-1, -1), cvTermCriteria(CV_TERMCRIT_EPS + CV_TERMCRIT_ITER, 30, 0.1));  
        cvDrawChessboardCorners(cam_image, cam_board_sz, cam_corners, cam_corner_count, cam_found);  
  
        if (cam_corner_count != cam_board_n)  
            cout << "find cam"<<ii<<"  corner failed!\n";      
        //when cam and prj are success store the result  
        if ( cam_corner_count == cam_board_n) 
		{  
            //store cam result  
            cam_step = successes*cam_board_n;  
            for (int i = cam_step, j = 0; j < cam_board_n; ++i, ++j) 
			{  
                CV_MAT_ELEM(*cam_image_points, float, i, 0) = cam_corners[j].x;  
                CV_MAT_ELEM(*cam_image_points, float, i, 1) = cam_corners[j].y;  
                CV_MAT_ELEM(*cam_object_points, float, i, 0) = (j/cam_board_w)*cam_Dx;  
                CV_MAT_ELEM(*cam_object_points, float, i, 1) = (j % cam_board_w)*cam_Dy;  
                CV_MAT_ELEM(*cam_object_points, float, i, 2) = 0.0f;  
                cam_data << cam_corners[j].x << "\t" << cam_corners[j].y << "\n";  
                cam_object_data << (j/cam_board_w)*cam_Dx << "\t" << (j %cam_board_w)*cam_Dy << "\t0\n";  
            }  
            CV_MAT_ELEM(*cam_point_counts, int, successes, 0) = cam_board_n;  
            successes++;  
            cout << "success number" << successes << endl;  
            cvShowImage("calibration", cam_image);  
            cvWaitKey(500);  
			k++;
        }
    } 
    if (successes < 2)  
        exit(0);    
    cam_image_points2 = cvCreateMat(cam_board_n*(successes), 2, CV_32FC1);  
    cam_object_points2 = cvCreateMat(cam_board_n*(successes), 3, CV_32FC1);  
    CvMat*cam_point_counts2 = cvCreateMat((successes), 1, CV_32SC1);  
    for (int i = 0; i < successes*cam_board_n; ++i){  
        CV_MAT_ELEM(*cam_image_points2, float, i, 0) = CV_MAT_ELEM(*cam_image_points, float, i, 0);  
        CV_MAT_ELEM(*cam_image_points2, float, i, 1) = CV_MAT_ELEM(*cam_image_points, float, i, 1);  
        CV_MAT_ELEM(*cam_object_points2, float, i, 0) = CV_MAT_ELEM(*cam_object_points, float, i, 0);  
        CV_MAT_ELEM(*cam_object_points2, float, i, 1) = CV_MAT_ELEM(*cam_object_points, float, i, 1);  
        CV_MAT_ELEM(*cam_object_points2, float, i, 2) = CV_MAT_ELEM(*cam_object_points, float, i, 2);  
  
    }  
    for (int i = 0; i < successes; ++i){  
        CV_MAT_ELEM(*cam_point_counts2, int, i, 0) = CV_MAT_ELEM(*cam_point_counts, int, i, 0);  
    }  
    cvSave("E:/复杂机电/单目标定图片\\cam_corners.xml", cam_image_points2);  
    cvReleaseMat(&cam_object_points);  
    cvReleaseMat(&cam_image_points);  
    cvReleaseMat(&cam_point_counts);  
    //calibration for cam
    CV_MAT_ELEM(*cam_intrinsic_matrix, float, 0, 0) = 1.0f;  
    CV_MAT_ELEM(*cam_intrinsic_matrix, float, 1, 1) = 1.0f;  
    CvMat* cam_rotation_all = cvCreateMat( successes, 3, CV_32FC1);  
    CvMat* cam_translation_vector_all = cvCreateMat( successes,3, CV_32FC1);  
    cvCalibrateCamera2(  
        cam_object_points2,  
        cam_image_points2,  
        cam_point_counts2,  
        //cam_image_sz,  
		cvGetSize(cam_image_temp),
        cam_intrinsic_matrix,  
        cam_distortion_coeffs,  
        cam_rotation_all,  
        cam_translation_vector_all,  
        0//CV_CALIB_FIX_ASPECT_RATIO    
        );  
    cvSave("E:/复杂机电/单目标定图片\\cam_intrinsic_matrix.txt", cam_intrinsic_matrix);  
    cvSave("E:/复杂机电/单目标定图片\\cam_distortion_coeffs.txt", cam_distortion_coeffs);  
    cvSave("E:/复杂机电/单目标定图片\\cam_rotation_all.txt", cam_rotation_all);  
    cvSave("E:/复杂机电/单目标定图片\\cam_translation_vector_all.txt", cam_translation_vector_all);  
    //char path1[100] = "E:/复杂机电/单目标定图片\\RGB_CALIBRATION.txt";
	 char path1[100] = "E:/复杂机电/单目标定图片\\IR_CALIBRATION.txt";  
	CvMat *intrinsic=(CvMat*)cvLoad("E:/复杂机电/单目标定图片\\cam_intrinsic_matrix.txt");
	CvMat *distortion=(CvMat*)cvLoad("E:/复杂机电/单目标定图片\\test\\cam_distortion_coeffs.txt");
	 IplImage *mapx=cvCreateImage(cam_image_sz,IPL_DEPTH_32F,1);
	 IplImage *mapy=cvCreateImage(cam_image_sz,IPL_DEPTH_32F,1);
	 cvInitUndistortMap(intrinsic,distortion,mapx,mapy);
	 //cvNamedWindow("原始图像",1);  
     //cvNamedWindow("非畸变图像",1);   
	 IplImage * clone=cvCloneImage(cam_image_temp);  
	 cvShowImage("原始图像",cam_image_temp); 
	 cvRemap(clone,cam_image_temp,mapx,mapy);  
     cvReleaseImage(&clone);  
	 cvShowImage("非畸变图像",cam_image_temp);
	 cvWaitKey(500);
    save_result(cam_rotation_all, cam_translation_vector_all,  
        cam_intrinsic_matrix, cam_distortion_coeffs,path1,successes);  
	return 0;
}  
void save_result(CvMat*cam_rotation_all, CvMat*cam_translation_vector_all,  
    CvMat*cam_intrinsic_matrix, CvMat*cam_distortion_coeffs,char*pathc,int sucesses)  
{  
    fstream Yeah_result;  
    Yeah_result.open(pathc, ofstream::out);  
    Yeah_result << setprecision(12) << "fc[0] =" << CV_MAT_ELEM(*cam_intrinsic_matrix, float, 0, 0  
        ) << "; fc[1] =" << CV_MAT_ELEM(*cam_intrinsic_matrix, float, 1, 1) << "; //CAM的焦距\n";  
    Yeah_result  << setprecision(12) << "cc[0] = " << CV_MAT_ELEM(*cam_intrinsic_matrix, float, 0, 2)   
        << "; cc[1] = " << CV_MAT_ELEM(*cam_intrinsic_matrix, float, 1, 2) << ";//CAM中心点\n";  
    Yeah_result  << setprecision(12) << "kc[0] =" << CV_MAT_ELEM(*cam_distortion_coeffs, float, 0, 0) <<  
        "; kc[1] =" << CV_MAT_ELEM(*cam_distortion_coeffs, float, 1, 0) <<  
        ";  kc[2] =" << CV_MAT_ELEM(*cam_distortion_coeffs, float, 2, 0) <<   
        ";  kc[3] =" << CV_MAT_ELEM(*cam_distortion_coeffs, float, 3, 0)   
        << ";  kc[4] =0;//畸变参数，请参照MATLAB里的定义\n\n外参数:\n";  
        for(int i=0;i<sucesses;++i)  
        {  
            Yeah_result<<"r:("<<setprecision(12) <<CV_MAT_ELEM(*cam_rotation_all, float, i, 0)<<"\t,"<<CV_MAT_ELEM(*cam_rotation_all, float, i, 1)<<"\t,"<<CV_MAT_ELEM(*cam_rotation_all, float, i, 2)<<")\n";  
            Yeah_result<<"t:("<<setprecision(12) <<CV_MAT_ELEM(*cam_translation_vector_all, float, i, 0)<<"\t,"<<CV_MAT_ELEM(*cam_translation_vector_all, float, i, 1)<<"\t,"<<CV_MAT_ELEM(*cam_translation_vector_all, float, i, 2)<<")\n\n\n";  
        }  
}  