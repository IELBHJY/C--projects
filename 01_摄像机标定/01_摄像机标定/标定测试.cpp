/*******************************************************************************************************************
//������:  
        ������궨���� 
��������:  
        OpenCv2.4.8+VS2012  
ʱ��ص�:  
        ����ʦ����ѧ 2016.10.8 
������Ϣ:  
        ����  
***********************************************************************************************************************/    
/*************************************************��������ͷ�ļ��������ռ䡿*******************************************/    
#include "opencv2/core/core.hpp"  
#include "opencv2/imgproc/imgproc.hpp"  
#include "opencv2/calib3d/calib3d.hpp"  
#include "opencv2/highgui/highgui.hpp"  
#include <cctype>  
#include <stdio.h>  
#include <string.h>  
#include <time.h>  
  
using namespace cv;  
using namespace std;  
  
  
const char * usage =  
" \nexample command line for calibration from a live feed.\n"             //��1��ʹ��һ�������ߵ������������������궨��ʱ��������в����������ʽ:  
"   calibration  -w 4 -h 5 -s 0.025 -o camera.yml -op -oe\n"              //��2����ʽ������ʾ:calibration  -w 4 -h 5 -s 0.025 -o camera.yml -op -oe  
" \n"  
" example command line for calibration from a list of stored images:\n"   //��1��ʹ�á�ͼƬ���С�����������궨������������еĸ�ʽ:  
"   imagelist_creator image_list.xml *.png\n"                             //��2��imagelist_creator image_list.xml *.png  
"   calibration -w 4 -h 5 -s 0.025 -o camera.yml -op -oe image_list.xml\n"//��3��calibration -w 4 -h 5 -s 0.025 -o camera.yml -op -oe image_list.xml  
" where image_list.xml is the standard OpenCV XML/YAML\n"                 //��1��image_list.xml��һ����׼��OpenCv XML/YXML�ļ�  
" use imagelist_creator to create the xml or yaml list\n"                 //��2��ʹ��imagelist_creatorȥ�������XML/YXML�б��ļ�  
" file consisting of the list of strings, e.g.:\n"  
" \n"  
"<?xml version=\"1.0\"?>\n"  
"<opencv_storage>\n"  
"<images>\n"  
"view000.png\n"  
"view001.png\n"  
"<!-- view002.png -->\n"  
"view003.png\n"  
"view010.png\n"   
"one_extra_view.jpg\n"  
"</images>\n"  
"</opencv_storage>\n";  
  
  
  
  
const char* liveCaptureHelp =  
    "When the live video from camera is used as input, the following hot-keys may be used:\n"  
        "  <ESC>, 'q' - quit the program\n"  
        "  'g' - start capturing images\n"  
        "  'u' - switch undistortion on/off\n";  
/****************************************************************************************************************************  
��������:  
       �����������  
��������:  
       �� 
��������ֵ:  
       ��  
****************************************************************************************************************************/    
static void help()  
{  
    printf( "This is a camera calibration sample.\n"  
        "Usage: calibration\n"  
        "     -w <board_width>         # the number of inner corners per one of board dimension\n"        //��1�����̸�Ŀ��  
        "     -h <board_height>        # the number of inner corners per another board dimension\n"       //��2�����̸�ĸ߶�  
        "     [-pt <pattern>]          # the type of pattern: chessboard or circles' grid\n"              //��3�����̸��ģʽ---���������̸�/Բ�������̸�  
        "     [-n <number_of_frames>]  # the number of frames to use for calibration\n"                   //��4��Ϊ�˽���������궨,׼�������̸�ͼƬ������  
        "                              # (if not specified, it will be set to the number\n"  
        "                              #  of board views actually available)\n"  
        "     [-d <delay>]             # a minimum delay in ms between subsequent attempts to capture a next view\n"  
        "                              # (used only for video capturing)\n"  
        "     [-s <squareSize>]        # square size in some user-defined units (1 by default)\n"         //��5��һЩ�Ѿ����û����������̸񽹵�֮��ľ����СSize  
        "     [-o <out_camera_params>] # the output filename for intrinsic [and extrinsic] parameters\n"  //��6������������ڲ��������ⲿ�������ļ���  
        "     [-op]                    # write detected feature points\n"                                 //��7��д��⵽��������  
        "     [-oe]                    # write extrinsic parameters\n"                                    //��8��д������궨���ⲿ����  
        "     [-zt]                    # assume zero tangential distortion\n"                             //��9������û���������  
        "     [-a <aspectRatio>]       # fix aspect ratio (fx/fy)\n"                                      //��10���̶������  
        "     [-p]                     # fix the principal point at the center\n"                         //��11����ס��̶���ͼ�������  
        "     [-v]                     # flip the captured images around the horizontal axis\n"           //��12������ȡ��ͼƬ��ת��ˮƽ������  
        "     [-V]                     # use a video file, and not an image list, uses\n"                 //��13��ʹ�õ���һ����Ƶ�ļ�������ͼ������  
        "                              # [input_data] string for the video file name\n"  
        "     [-su]                    # show undistorted images after calibration\n"                     //��14����������궨֮����ʾ�������֮���ͼƬ  
        "     [input_data]             # input data, one of the following:\n"                             //��15����������������ʾ:  
        "                              #  - text file with a list of the images of the board\n"                     //[1]�洢���̸�궨ͼƬ���е��ı��ļ�  
        "                              #    the text file can be generated with imagelist_creator\n"                //[2]����ļ�����ʹ��imagelist_ceator����  
        "                              #  - name of video file with a video of the board\n"                         //[3]һ���궨����Ƶ�ļ����ļ���  
        "                              # if input_data not specified, a live view from the camera is used\n"        //[4]���������ļ�û�б�ָ��,��ô��������������߲������̸�궨ͼƬ  
        "\n" );  
    printf("\n%s",usage);                                                                                  //��1������û�������Ϣ  
    printf( "\n%s", liveCaptureHelp );                                                                     //��2��ʹ����������߱궨��ʱ�����ʾ��Ϣ  
}  
  
enum { DETECTION = 0, CAPTURING = 1, CALIBRATED = 2 };  
enum Pattern { CHESSBOARD, CIRCLES_GRID, ASYMMETRIC_CIRCLES_GRID };  
/****************************************************************************************************************************  
��������:  
       ���������궨֮�󣬶�������궨�Ľ���������ۣ�������ͶӰ���/������궨��� 
��������:  
       1---const vector<vector<Point3f> >& objectPoints----����������----��ά����ϵ�µ���������� 
       2---const vector<vector<Point2f> >& imagePoints ----ͼ������ϵ�µĶ�ά�����----������ 
       3---const Mat& cameraMatrix-------------------------������ڲξ��� 
       4---const Mat& distCoeffs---------------------------������������� 
       5---const vector<Mat>&              rvecs-----------��ת���� 
       6---const vector<Mat>&              tvecs-----------ƽ�ƾ��� 
��������ֵ:  
       ����� 
****************************************************************************************************************************/    
static double computeReprojectionErrors(               
        const vector<vector<Point3f> >& objectPoints,  
        const vector<vector<Point2f> >& imagePoints,  
        const vector<Mat>&              rvecs,   
        const vector<Mat>&              tvecs,  
        const Mat&                      cameraMatrix,   
        const Mat&                      distCoeffs,  
        vector<float>&                  perViewErrors )  
{  
    vector<Point2f> imagePoints2;  
    int i              = 0;  
    int totalPoints    = 0;  
    double totalErr    = 0;                                                    //��1������ͼ���ƽ�����  
    double err         = 0;  
    perViewErrors.resize(objectPoints.size());  
    /*ͶӰ����--��ӦOpenCv1.0�汾�е�cvProjectPoints2()����---CCS--->ICS*/  
    for( i = 0; i < (int)objectPoints.size(); i++ )  
    {  
        projectPoints(Mat(objectPoints[i]),                                    //��1����ҪͶӰ�����������ϵ�µ���λ�������  
                          rvecs[i],                                            //��2��ƽ�ƾ���  
                      tvecs[i],                                            //��3����ת����  
                                      cameraMatrix,                                        //��4��������ڲ�������  
                      distCoeffs,                                          //��5���������������(�������,�������k1,k2,k3,p1,p2)  
                          imagePoints2);                                       //��6�������������ά�����������µ�λ��,���Ǽ�������ĸ���ά���ڳ������е�����(��������)  
                                                                           
        err              = norm(Mat(imagePoints[i]), Mat(imagePoints2), CV_L2);//��7�����������ӦԪ�ز�ֵƽ�����ۼӺ�  
        int n            = (int)objectPoints[i].size();                        //��8��Vector�����ĳ�Ա����--resize(),size(),push_back(),pop_back()                  
        perViewErrors[i] = (float)std::sqrt(err*err/n);                        //��9��������ά���ͶӰ���  
  
        totalErr    += err*err;  
        totalPoints += n;  
    }  
  
    return std::sqrt(totalErr/totalPoints);                                    //��10�������ͶӰ���������  
}  
/****************************************************************************************************************************  
����ԭ��: 
       static void calcChessboardCorners(Size boardSize, float squareSize, vector<Point3f>& corners, Pattern patternType = CHESSBOARD) 
��������:  
       ���������̸�----��������ϵ��----��ʵ��������ά���������� 
��������:  
       1---Size boardSize-------------���̸�ĳߴ�Size 
       2---float squareSize-----------���̸�ǵ�֮��ľ���Size 
       3---vector<Point3f>& corners---�����洢���̸�ǵ����ά���� 
       4---Pattern patternType--------�궨������� 
��������ֵ: 
       void 
****************************************************************************************************************************/    
static void calcChessboardCorners(Size boardSize, float squareSize, vector<Point3f>& corners, Pattern patternType = CHESSBOARD)  
{  
    corners.resize(0);                                               //��1��Vector�����ĳ�Ա����,����Vector�����ĳ���Ϊ0  
  
    switch(patternType)                                              //��2���жϱ궨�������  
    {  
      case CHESSBOARD:                                               //��3�����̸����͵ı궨��  
      case CIRCLES_GRID:                                             //��4��Բ���ͷ��ı궨��  
        for( int i = 0; i < boardSize.height; i++ )  
            for( int j = 0; j < boardSize.width; j++ )  
                corners.push_back(Point3f(float(j*squareSize),       //��5�������������ϵ�µ���ά����ʵ�ʵı궨�����������洢��corners���vector����������  
                                          float(i*squareSize), 0));  
        break;  
  
      case ASYMMETRIC_CIRCLES_GRID:  
        for( int i = 0; i < boardSize.height; i++ )  
            for( int j = 0; j < boardSize.width; j++ )  
                corners.push_back(Point3f(float((2*j + i % 2)*squareSize),  
                                          float(i*squareSize), 0));  
        break;  
  
      default:  
        CV_Error(CV_StsBadArg, "Unknown pattern type\n");  
    }  
}  
/****************************************************************************************************************************  
����ԭ��: 
       static bool runCalibration(�ԣ� 
��������:  
       ������궨ģ������ĵ�ģ��--------������궨���� 
��������:  
       vector<vector<Point2f> > imagePoints,          //��1��������Ʊ���---WCS����ά������3D��������,�Լ��ֶ��Ƶ���������λ������ 
       Size                     imageSize,            //��2��������Ʊ���---ICS����ϵ��,����findChessboardCorners()������������ı궨ͼƬ�Ͻǵ������ 
       Size                     boardSize,            //��3��������Ʊ���---���̸��Size---���̸�ĺ��ݡ��ڽǵ㡿���� 
       Pattern                  patternType,          //��4��������Ʊ���---���������̸�,ԭ�������̸�,�궨��ģʽ 
       float                    squareSize,           //��5��������Ʊ���---���̸�ǵ�֮��ľ���/Բ���ͱ궨��Բ��֮��ľ��� 
       float                    aspectRatio,          //��6��������Ʊ���---����� 
       int                      flags,                //��7��������Ʊ���---��־λ 
       Mat&                     cameraMatrix,         //��1�������������Ʊ��� ---������ڲ������� 
       Mat&                     distCoeffs,           //��2�������������Ʊ��� ---���������ϵ������ 
       vector<Mat>&             rvecs,                //��3�������������Ʊ���---��ת���� 
       vector<Mat>&             tvecs,                //��4�������������Ʊ���---��������(k1,k2,k3,p1,p2) 
       vector<float>&           reprojErrs,           //��5�������������Ʊ���---����ͼƬ/����ͶӰ���---ͶӰ���/������궨��� 
       double&                  totalAvgErr           //��6�������������Ʊ���---������궨������ƽ�����/ͶӰƽ����� 
��������ֵ: 
       ������궨�Ƿ�ɹ�-----����������������������Ƿ�ɹ� 
****************************************************************************************************************************/    
static bool runCalibration( vector<vector<Point2f> > imagePoints,  
                            Size                     imageSize,   
                            Size                     boardSize,   
                            Pattern     
                              
                            patternType,  
                            float                    squareSize,   
                            float                    aspectRatio,  
                            int                      flags,   
                            Mat&                     cameraMatrix,   
                            Mat&                     distCoeffs,  
                            vector<Mat>&             rvecs,   
                            vector<Mat>&             tvecs,  
                            vector<float>&           reprojErrs,  
                            double&                  totalAvgErr)  
{  
    cameraMatrix = Mat::eye(3, 3, CV_64F);                                         //��1��������ڲ���������,����һ��3*3�ĵ�λ����  
  
    if( flags & CV_CALIB_FIX_ASPECT_RATIO )  
        cameraMatrix.at<double>(0,0) = aspectRatio;  
  
    distCoeffs = Mat::zeros(8, 1, CV_64F);                                         //��2��������Ļ���ϵ������,����һ��8*1��������  
  
    vector<vector<Point3f> > objectPoints(1);  
  
    calcChessboardCorners(boardSize, squareSize, objectPoints[0], patternType);    //��3���������̸�ǵ���������ϵ�µ���ά��������  
  
    objectPoints.resize(imagePoints.size(),objectPoints[0]);                       //��4����objectPoints��Vector������������,����������ڴ�ռ���Ԫ��objectPoints[0]���  
                                                                                   //��5��������궨����----������������ڲ��������ⲿ����  
    double rms = calibrateCamera(objectPoints,      //��1����������ϵ��*ÿ�ű궨ͼƬ�еĽǵ������k*ͼƬ�ĸ���M---N*3����(N=k*M)------��������                                 
                                 imagePoints,       //��2��imagePoints��һ��N*2�ľ���,����objectPoints���ṩ�����е�����Ӧ����������깹��,  
                                                          //���ʹ�����̸���б궨,��ô����������򵥵���M�ε���cvFindChessboardCorners()�ķ���ֵ����  
                                 imageSize,         //��3��imageSize�������غ�����ͼ��ĳߴ�Size,ͼ�����ǴӸ�ͼ������ȡ��  
                                 cameraMatrix,      //��4��������ڲ���������--------������������������������Ϊ  
                                 distCoeffs,        //��5������ϵ��������5*1---8*1---����ı�����������ķ�������Ϊ  
                                 rvecs,             //��6��rotation_vectors----------��ת����  
                                 tvecs,             //��7��tanslation_vectors--------ƽ�ƾ���  
                                 flags|CV_CALIB_FIX_K4|CV_CALIB_FIX_K5);          ///*|CV_CALIB_FIX_K3*/|CV_CALIB_FIX_K4|CV_CALIB_FIX_K5);  
  
    printf("RMS error reported by calibrateCamera: %g\n", rms);  
  
    bool ok = checkRange(cameraMatrix) && checkRange(distCoeffs);                  //��6��checkRange()����---���ڼ������е�ÿһ��Ԫ���Ƿ���ָ����һ����ֵ����֮��  
  
    totalAvgErr = computeReprojectionErrors(objectPoints, imagePoints,             //��7�����������궨�󣬶Ա궨�������ۣ�������ͶӰ���/������궨���  
                rvecs, tvecs, cameraMatrix, distCoeffs, reprojErrs);               //��8�������ķ���ֵ��������궨/ͶӰ������ƽ�����  
  
    return ok;  
}  
  
  
static void saveCameraParams( const string& filename,  
                       Size imageSize, Size boardSize,  
                       float squareSize, float aspectRatio, int flags,  
                       const Mat& cameraMatrix, const Mat& distCoeffs,  
                       const vector<Mat>& rvecs, const vector<Mat>& tvecs,  
                       const vector<float>& reprojErrs,  
                       const vector<vector<Point2f> >& imagePoints,  
                       double totalAvgErr )  
{  
    FileStorage fs( filename, FileStorage::WRITE );  
  
    time_t tt;  
    time( &tt );  
    struct tm *t2 = localtime( &tt );  
    char buf[1024];  
    strftime( buf, sizeof(buf)-1, "%c", t2 );  
  
    fs << "calibration_time" << buf;  
  
    if( !rvecs.empty() || !reprojErrs.empty() )  
        fs << "nframes" << (int)std::max(rvecs.size(), reprojErrs.size());  
    fs << "image_width" << imageSize.width;  
    fs << "image_height" << imageSize.height;  
    fs << "board_width" << boardSize.width;  
    fs << "board_height" << boardSize.height;  
    fs << "square_size" << squareSize;  
  
    if( flags & CV_CALIB_FIX_ASPECT_RATIO )  
        fs << "aspectRatio" << aspectRatio;  
  
    if( flags != 0 )  
    {  
        sprintf( buf, "flags: %s%s%s%s",  
            flags & CV_CALIB_USE_INTRINSIC_GUESS ? "+use_intrinsic_guess" : "",  
            flags & CV_CALIB_FIX_ASPECT_RATIO ? "+fix_aspectRatio" : "",  
            flags & CV_CALIB_FIX_PRINCIPAL_POINT ? "+fix_principal_point" : "",  
            flags & CV_CALIB_ZERO_TANGENT_DIST ? "+zero_tangent_dist" : "" );  
        cvWriteComment( *fs, buf, 0 );  
    }  
  
    fs << "flags" << flags;  
  
    fs << "camera_matrix" << cameraMatrix;  
    fs << "distortion_coefficients" << distCoeffs;  
  
    fs << "avg_reprojection_error" << totalAvgErr;  
    if( !reprojErrs.empty() )  
        fs << "per_view_reprojection_errors" << Mat(reprojErrs);  
  
    if( !rvecs.empty() && !tvecs.empty() )  
    {  
        CV_Assert(rvecs[0].type() == tvecs[0].type());  
        Mat bigmat((int)rvecs.size(), 6, rvecs[0].type());  
        for( int i = 0; i < (int)rvecs.size(); i++ )  
        {  
            Mat r = bigmat(Range(i, i+1), Range(0,3));  
            Mat t = bigmat(Range(i, i+1), Range(3,6));  
  
            CV_Assert(rvecs[i].rows == 3 && rvecs[i].cols == 1);  
            CV_Assert(tvecs[i].rows == 3 && tvecs[i].cols == 1);  
            //*.t() is MatExpr (not Mat) so we can use assignment operator  
            r = rvecs[i].t();  
            t = tvecs[i].t();  
        }  
        cvWriteComment( *fs, "a set of 6-tuples (rotation vector + translation vector) for each view", 0 );  
        fs << "extrinsic_parameters" << bigmat;  
    }  
  
    if( !imagePoints.empty() )  
    {  
        Mat imagePtMat((int)imagePoints.size(), (int)imagePoints[0].size(), CV_32FC2);  
        for( int i = 0; i < (int)imagePoints.size(); i++ )  
        {  
            Mat r = imagePtMat.row(i).reshape(2, imagePtMat.cols);  
            Mat imgpti(imagePoints[i]);  
            imgpti.copyTo(r);  
        }  
        fs << "image_points" << imagePtMat;  
    }  
}  
/****************************************************************************************************************************   
����ԭ��:  
       static bool readStringList( const string& filename, vector<string>& l )  
��������:   
       ��ȡ�ַ����б��ļ��е�����  
��������:   
       1---const string& filename-----Ҫ��ȡ��XML�ļ����ļ���  
       2---cvector<string>& l---------���ļ��ж�ȡ��������,�洢��vector�����Ķ���l��  
��������ֵ:  
       �ɹ�����true  
****************************************************************************************************************************/    
static bool readStringList( const string& filename, vector<string>& l )  
{  
    l.resize(0);                                      //[1]���������ĳ���resize()  
    FileStorage fs(filename, FileStorage::READ);      //[2]ʹ��OpenCv�е�FileStorage�ļ��洢���ȡxml�ļ�  
    if( !fs.isOpened() )                              //[3]�ж��ļ��Ƿ��Ѿ���,����Ϊtrue  
        return false;  
    FileNode n = fs.getFirstTopLevelNode();           //[4]���ض���ӳ��ĵ�һ���ڵ�,FileNode---�ļ��ڵ�����  
    if( n.type() != FileNode::SEQ )                   //[5]�ļ��ڵ�������ǲ�������Sequence--SEQ  
        return false;  
    FileNodeIterator it = n.begin(), it_end = n.end();//[6]�����ڵ�  
    for( ; it != it_end; ++it )  
        l.push_back((string)*it);  
    return true;  
}  
  
  
static bool runAndSave(const string&                   outputFilename,  
                       const vector<vector<Point2f> >& imagePoints,  
                       Size                            imageSize,   
                       Size                            boardSize,   
                       Pattern                         patternType,   
                       float                           squareSize,  
                       float                           aspectRatio,  
                       int                             flags,   
                       Mat&                            cameraMatrix,  
                       Mat&                            distCoeffs,   
                       bool                            writeExtrinsics,   
                       bool                            writePoints )  
{  
    vector<Mat> rvecs, tvecs;  
    vector<float> reprojErrs;  
    double totalAvgErr = 0;  
  
    bool ok = runCalibration(imagePoints,   
                             imageSize,  
                             boardSize,   
                             patternType,   
                             squareSize,  
                             aspectRatio,   
                             flags,   
                             cameraMatrix,   
                             distCoeffs,  
                             rvecs,   
                             tvecs,   
                             reprojErrs,   
                             totalAvgErr);  
  
    printf("%s. avg reprojection error = %.2f\n",ok ? "Calibration succeeded" : "Calibration failed",totalAvgErr);  
  
    if( ok )  
        saveCameraParams( outputFilename,   
                          imageSize,  
                          boardSize,  
                          squareSize,   
                          aspectRatio,  
                          flags,   
                          cameraMatrix,   
                          distCoeffs,  
                          writeExtrinsics ? rvecs : vector<Mat>(),  
                          writeExtrinsics ? tvecs : vector<Mat>(),  
                          writeExtrinsics ? reprojErrs : vector<float>(),  
                          writePoints ? imagePoints : vector<vector<Point2f> >(),  
                          totalAvgErr );  
    return ok;  
}  
/*****************************************************��Main������************************************************************  
*      ����̨Ӧ�ó������� 
*****************************************************************************************************************************/  
int main( int argc, char** argv )  
{  
    Size boardSize;                                          //[1]�궨���Size  
    Size imageSize;                                          //[2]ͼƬ��Size  
  
    float squareSize  = 1.f;                                 //[3]���̸�ǵ�֮��ľ���  
    float aspectRatio = 1.f;                                 //[4]�����  
  
    Mat   cameraMatrix;                                      //[5]��������ڲ�������  
    Mat   distCoeffs;                                        //[6]������Ļ���ϵ������  
    const char* outputFilename = "out_camera_data.yml";      //[7]�����Xml�ļ���  
    const char* inputFilename  = 0;  
  
    int   i;  
    int   nframes           = 10;  
    bool  writeExtrinsics   = false;  
    bool  writePoints       = false;  
    bool  undistortImage    = false;  
    int   flags             = 0;  
  
  
    cv::VideoCapture        capture;                         //[8]����һ����Ƶ��������  
  
  
    bool  flipVertical      = false;  
    bool  showUndistorted   = false;  
    bool  videofile         = false;  
    int   delay             = 1000;  
    clock_t prevTimestamp   = 0;                             //[9]clock_t----#include<time.h>  
  
    int mode = DETECTION;  
    int cameraId = 0;  
  
    vector<vector<Point2f> > imagePoints;  
    vector<string>           imageList;  
  
    Pattern pattern         = CHESSBOARD;  
    //=============================================================================================  
    //��ģ��1�����û���κβ���������,����ʾ������Ϣ  
    //=============================================================================================  
    if( argc < 2 )  
    {  
        help();  
        return 0;  
    }  
    //=============================================================================================  
    //��ģ��2��ѭ����ʾ����Ĳ���,���ж�ÿһ������������  
    //=============================================================================================  
    for( i = 1; i < argc; i++ )  
    {  
        const char* s = argv[i];                                                            //[1]�ַ�����----�洢---�ַ���  
        if( strcmp( s, "-w" ) == 0 )                                                        //[2]�궨��Ŀ��  
        {  
            if( sscanf( argv[++i], "%u", &boardSize.width ) != 1 || boardSize.width <= 0 )  
                return fprintf( stderr, "Invalid board width\n" ), -1;                      //[3]���Ǹ����ű��ʽ  
        }  
        else if( strcmp( s, "-h" ) == 0 )                                                   //[2]�궨��ĸ߶�  
        {  
            if( sscanf( argv[++i], "%u", &boardSize.height ) != 1 || boardSize.height <= 0 )  
                return fprintf( stderr, "Invalid board height\n" ), -1;  
        }  
        else if( strcmp( s, "-pt" ) == 0 )                                                  //[3]���̸�ģʽ  
        {  
            i++;  
            if( !strcmp( argv[i], "circles" ) )  
                pattern = CIRCLES_GRID;  
            else if( !strcmp( argv[i], "acircles" ) )  
                pattern = ASYMMETRIC_CIRCLES_GRID;  
            else if( !strcmp( argv[i], "chessboard" ) )  
                pattern = CHESSBOARD;  
            else  
                return fprintf( stderr, "Invalid pattern type: must be chessboard or circles\n" ), -1;  
        }  
        else if( strcmp( s, "-s" ) == 0 )                                                  //[4]���̸�ǵ�֮��ľ���  
        {  
            if( sscanf( argv[++i], "%f", &squareSize ) != 1 || squareSize <= 0 )  
                return fprintf( stderr, "Invalid board square width\n" ), -1;  
        }  
        else if( strcmp( s, "-n" ) == 0 )                                                  //[5]ͼƬ����  
        {  
            if( sscanf( argv[++i], "%u", &nframes ) != 1 || nframes <= 3 )  
                return printf("Invalid number of images\n" ), -1;  
        }  
        else if( strcmp( s, "-a" ) == 0 )                                                  //[6]�����  
        {  
            if( sscanf( argv[++i], "%f", &aspectRatio ) != 1 || aspectRatio <= 0 )  
                return printf("Invalid aspect ratio\n" ), -1;  
            flags |= CV_CALIB_FIX_ASPECT_RATIO;  
        }  
        else if( strcmp( s, "-d" ) == 0 )                                                  //[7]�ӳ�ʱ��  
        {  
            if( sscanf( argv[++i], "%u", &delay ) != 1 || delay <= 0 )  
                return printf("Invalid delay\n" ), -1;  
        }  
        else if( strcmp( s, "-op" ) == 0 )  
        {  
            writePoints = true;  
        }  
        else if( strcmp( s, "-oe" ) == 0 )  
        {  
            writeExtrinsics = true;  
        }  
        else if( strcmp( s, "-zt" ) == 0 )  
        {  
            flags |= CV_CALIB_ZERO_TANGENT_DIST;  
        }  
        else if( strcmp( s, "-p" ) == 0 )  
        {  
            flags |= CV_CALIB_FIX_PRINCIPAL_POINT;  
        }  
        else if( strcmp( s, "-v" ) == 0 )  
        {  
            flipVertical = true;  
        }  
        else if( strcmp( s, "-V" ) == 0 )  
        {  
            videofile = true;  
        }  
        else if( strcmp( s, "-o" ) == 0 )  
        {  
            outputFilename = argv[++i];  
        }  
        else if( strcmp( s, "-su" ) == 0 )  
        {  
            showUndistorted = true;  
        }  
        else if( s[0] != '-' )  
        {  
            if( isdigit(s[0]) )  
                sscanf(s, "%d", &cameraId);  
            else  
                inputFilename = s;  
        }  
        else  
            return fprintf( stderr, "Unknown option %s", s ), -1;  
    }  
    //=============================================================================================  
    //��ģ��3����ȡ�����ļ����ļ���  
    //=============================================================================================  
    if( inputFilename )  
    {  
        if( !videofile && readStringList(inputFilename, imageList) )                       //[8]�Զ��庯��readStringList()  
            mode = CAPTURING;  
        else  
            capture.open(inputFilename);  
    }  
    else  
        capture.open(cameraId);  
  
    if( !capture.isOpened() && imageList.empty() )                                         //[9]��������ͷ������Ƶ�ļ��Ƿ�򿪻���imagelist�Ƿ�Ϊ��  
        return fprintf( stderr, "Could not initialize video (%d) capture\n",cameraId ), -2;  
  
    if( !imageList.empty() )  
        nframes = (int)imageList.size();                                                   //[10]����vector����Ԫ�ص���Ŀ  
  
    if( capture.isOpened() )                                                               //[11]��������ͷ�Ƿ��  
        printf( "%s", liveCaptureHelp );  
  
    namedWindow( "Image View", 1 );                                                        //[12]����һ����Ƶ����  
  
    for(i = 0;;i++)  
    {  
        Mat view;  
        Mat viewGray;  
        bool blink = false;  
  
        if( capture.isOpened() )                                                           //[13]��������ͷ�Ƿ��               
        {  
            Mat view0;  
            capture >> view0;  
            view0.copyTo(view);  
        }  
        else if( i < (int)imageList.size() )                                               //[14]��ȡͼ�������еĶ��ļ�  
            view = imread(imageList[i], 1);  
    //=============================================================================================  
    //��ģ��4������������궨,���ұ���������궨�Ľ��  
    //=============================================================================================  
          
        if(!view.data)                                            
        {  
            if( imagePoints.size() > 0 )  
                runAndSave(outputFilename,  
                           imagePoints,   
                           imageSize,  
                           boardSize,   
                           pattern,   
                           squareSize,   
                           aspectRatio,  
                           flags,   
                           cameraMatrix,   
                           distCoeffs,  
                           writeExtrinsics,   
                           writePoints);  
            break;  
        }  
  
        imageSize = view.size();  
  
        if( flipVertical )  
            flip( view, view, 0 );  
        /** 
        *ʹ��ͼƬ���еı궨���� 
        **/  
        vector<Point2f> pointbuf;  
        cvtColor(view, viewGray, COLOR_BGR2GRAY);  
  
        bool found;  
          
        switch( pattern )                                                //��1��ʹ��findChessboardCorners()����Ѱ�����̸�궨���Ͻǵ��������Ϣ---imagePoints  
        {  
            case CHESSBOARD:                                                              
                found = findChessboardCorners( view, boardSize, pointbuf, CV_CALIB_CB_ADAPTIVE_THRESH | CV_CALIB_CB_FAST_CHECK | CV_CALIB_CB_NORMALIZE_IMAGE);  
                break;  
            case CIRCLES_GRID:  
                found = findCirclesGrid( view, boardSize, pointbuf );  
                break;  
            case ASYMMETRIC_CIRCLES_GRID:  
                found = findCirclesGrid( view, boardSize, pointbuf, CALIB_CB_ASYMMETRIC_GRID );  
                break;  
            default:  
                return fprintf( stderr, "Unknown pattern type\n" ), -1;  
        }  
  
        if( pattern == CHESSBOARD && found)                              //��2��ʹ��cornerSubPix()�������ǵ����꾫ȷ�������ؼ���  
            cornerSubPix( viewGray, pointbuf, Size(11,11),Size(-1,-1), TermCriteria( CV_TERMCRIT_EPS+CV_TERMCRIT_ITER, 30, 0.1 ));  
  
        if( mode == CAPTURING && found &&(!capture.isOpened() || clock() - prevTimestamp > delay*1e-3*CLOCKS_PER_SEC) )  
        {  
            imagePoints.push_back(pointbuf);                            //��3���������еı궨���ϵĽǵ�����ת����imagePoints������  
            prevTimestamp = clock();  
            blink         = capture.isOpened();  
        }  
          
        if(found)                                                       //��4��ʹ��drawChessboardCorners()�����Ѿ��ҳ��������̸�ǵ�  
            drawChessboardCorners( view, boardSize, Mat(pointbuf), found );  
  
        string msg = mode == CAPTURING ? "100/100" :mode == CALIBRATED ? "Calibrated" : "Press 'g' to start";  
  
        int   baseLine = 0;  
        Size  textSize = getTextSize(msg, 1, 1, 1, &baseLine);  
        Point textOrigin(view.cols - 2*textSize.width - 10, view.rows - 2*baseLine - 10);  
  
        if( mode == CAPTURING )  
        {  
            if(undistortImage)  
                msg = format( "%d/%d Undist", (int)imagePoints.size(), nframes );  
            else  
                msg = format( "%d/%d",        (int)imagePoints.size(), nframes );  
        }  
  
        putText( view, msg, textOrigin, 1, 1,mode != CALIBRATED ? Scalar(0,0,255) : Scalar(0,255,0));  
  
        if( blink )  
            bitwise_not(view, view);  
  
        if( mode == CALIBRATED && undistortImage )  
        {  
            Mat temp = view.clone();  
            undistort(temp, view, cameraMatrix, distCoeffs);  
        }  
  
        imshow("Image View", view);  
        int key = 0xff & waitKey(capture.isOpened() ? 50 : 500);  
  
        if( (key & 255) == 27 )  
            break;  
  
        if( key == 'u' && mode == CALIBRATED )  
            undistortImage = !undistortImage;  
  
        if( capture.isOpened() && key == 'g' )  
        {  
            mode = CAPTURING;  
            imagePoints.clear();  
        }  
  
        if( mode == CAPTURING && imagePoints.size() >= (unsigned)nframes ) //��5����ʼ����������궨  
        {  
            if( runAndSave(outputFilename,  
                           imagePoints,   
                           imageSize,  
                           boardSize,   
                           pattern,   
                           squareSize,   
                           aspectRatio,  
                           flags,   
                           cameraMatrix,   
                           distCoeffs,  
                           writeExtrinsics,   
                           writePoints))  
                mode = CALIBRATED;  
            else  
                mode = DETECTION;  
            if( !capture.isOpened() )  
                break;  
        }  
    }  
                                                                           
    if( !capture.isOpened() && showUndistorted )                            //��6������ͼ��У��  
    {  
        Mat view, rview, map1, map2;  
  
        initUndistortRectifyMap(cameraMatrix,   
                                distCoeffs,   
                                Mat(),  
                                getOptimalNewCameraMatrix(cameraMatrix, distCoeffs, imageSize, 1, imageSize, 0),  
                                imageSize,   
                                CV_16SC2,   
                                map1,  
                                map2);  
  
        for( i = 0; i < (int)imageList.size(); i++ )  
        {  
            view = imread(imageList[i], 1);  
            if(!view.data)  
                continue;  
            //undistort( view, rview, cameraMatrix, distCoeffs, cameraMatrix );  
            remap(view, rview, map1, map2, INTER_LINEAR);  
            imshow("Image View", rview);  
            int c = 0xff & waitKey();  
            if( (c & 255) == 27 || c == 'q' || c == 'Q' )  
                break;  
        }  
    }  
    return 0;  
}