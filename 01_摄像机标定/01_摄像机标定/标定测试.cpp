/*******************************************************************************************************************
//程序功能:  
        摄像机标定程序 
开发环境:  
        OpenCv2.4.8+VS2012  
时间地点:  
        陕西师范大学 2016.10.8 
作者信息:  
        九月  
***********************************************************************************************************************/    
/*************************************************【函数的头文件和命名空间】*******************************************/    
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
" \nexample command line for calibration from a live feed.\n"             //【1】使用一个【在线的摄像机进行摄像机】标定的时候的命令行参数的输入格式:  
"   calibration  -w 4 -h 5 -s 0.025 -o camera.yml -op -oe\n"              //【2】格式如下所示:calibration  -w 4 -h 5 -s 0.025 -o camera.yml -op -oe  
" \n"  
" example command line for calibration from a list of stored images:\n"   //【1】使用【图片序列】进行摄像机标定的输入的命令行的格式:  
"   imagelist_creator image_list.xml *.png\n"                             //【2】imagelist_creator image_list.xml *.png  
"   calibration -w 4 -h 5 -s 0.025 -o camera.yml -op -oe image_list.xml\n"//【3】calibration -w 4 -h 5 -s 0.025 -o camera.yml -op -oe image_list.xml  
" where image_list.xml is the standard OpenCV XML/YAML\n"                 //【1】image_list.xml是一个标准的OpenCv XML/YXML文件  
" use imagelist_creator to create the xml or yaml list\n"                 //【2】使用imagelist_creator去创建这个XML/YXML列表文件  
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
函数功能:  
       程序帮助函数  
函数参数:  
       无 
函数返回值:  
       无  
****************************************************************************************************************************/    
static void help()  
{  
    printf( "This is a camera calibration sample.\n"  
        "Usage: calibration\n"  
        "     -w <board_width>         # the number of inner corners per one of board dimension\n"        //【1】棋盘格的宽度  
        "     -h <board_height>        # the number of inner corners per another board dimension\n"       //【2】棋盘格的高度  
        "     [-pt <pattern>]          # the type of pattern: chessboard or circles' grid\n"              //【3】棋盘格的模式---正方形棋盘格/圆心型棋盘格  
        "     [-n <number_of_frames>]  # the number of frames to use for calibration\n"                   //【4】为了进行摄像机标定,准备的棋盘格图片的数量  
        "                              # (if not specified, it will be set to the number\n"  
        "                              #  of board views actually available)\n"  
        "     [-d <delay>]             # a minimum delay in ms between subsequent attempts to capture a next view\n"  
        "                              # (used only for video capturing)\n"  
        "     [-s <squareSize>]        # square size in some user-defined units (1 by default)\n"         //【5】一些已经被用户定义了棋盘格焦点之间的距离大小Size  
        "     [-o <out_camera_params>] # the output filename for intrinsic [and extrinsic] parameters\n"  //【6】保存摄像机内部参数和外部参数的文件名  
        "     [-op]                    # write detected feature points\n"                                 //【7】写检测到的特征点  
        "     [-oe]                    # write extrinsic parameters\n"                                    //【8】写摄像机标定的外部参数  
        "     [-zt]                    # assume zero tangential distortion\n"                             //【9】假设没有切向畸变  
        "     [-a <aspectRatio>]       # fix aspect ratio (fx/fy)\n"                                      //【10】固定长宽比  
        "     [-p]                     # fix the principal point at the center\n"                         //【11】将住店固定在图像的中心  
        "     [-v]                     # flip the captured images around the horizontal axis\n"           //【12】将获取的图片翻转到水平坐标上  
        "     [-V]                     # use a video file, and not an image list, uses\n"                 //【13】使用的是一个视频文件而不是图像序列  
        "                              # [input_data] string for the video file name\n"  
        "     [-su]                    # show undistorted images after calibration\n"                     //【14】在摄像机标定之后显示畸变矫正之后的图片  
        "     [input_data]             # input data, one of the following:\n"                             //【15】输入数据如下所示:  
        "                              #  - text file with a list of the images of the board\n"                     //[1]存储棋盘格标定图片序列的文本文件  
        "                              #    the text file can be generated with imagelist_creator\n"                //[2]这个文件可以使用imagelist_ceator产生  
        "                              #  - name of video file with a video of the board\n"                         //[3]一个标定板视频文件的文件名  
        "                              # if input_data not specified, a live view from the camera is used\n"        //[4]如果输入的文件没有被指定,那么就是用摄像机在线捕获棋盘格标定图片  
        "\n" );  
    printf("\n%s",usage);                                                                                  //【1】输出用户帮助信息  
    printf( "\n%s", liveCaptureHelp );                                                                     //【2】使用摄像机在线标定的时候的提示信息  
}  
  
enum { DETECTION = 0, CAPTURING = 1, CALIBRATED = 2 };  
enum Pattern { CHESSBOARD, CIRCLES_GRID, ASYMMETRIC_CIRCLES_GRID };  
/****************************************************************************************************************************  
函数功能:  
       完成摄像机标定之后，对摄像机标定的结果进行评价，计算重投影误差/摄像机标定误差 
函数参数:  
       1---const vector<vector<Point3f> >& objectPoints----常引用类型----三维坐标系下的物理坐标点 
       2---const vector<vector<Point2f> >& imagePoints ----图像坐标系下的二维坐标点----成像仪 
       3---const Mat& cameraMatrix-------------------------摄像机内参矩阵 
       4---const Mat& distCoeffs---------------------------摄像机畸变向量 
       5---const vector<Mat>&              rvecs-----------旋转矩阵 
       6---const vector<Mat>&              tvecs-----------平移矩阵 
函数返回值:  
       误差率 
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
    double totalErr    = 0;                                                    //【1】单幅图像的平均误差  
    double err         = 0;  
    perViewErrors.resize(objectPoints.size());  
    /*投影函数--对应OpenCv1.0版本中的cvProjectPoints2()函数---CCS--->ICS*/  
    for( i = 0; i < (int)objectPoints.size(); i++ )  
    {  
        projectPoints(Mat(objectPoints[i]),                                    //【1】将要投影的摄像机坐标系下的三位点的坐标  
                          rvecs[i],                                            //【2】平移矩阵  
                      tvecs[i],                                            //【3】旋转矩阵  
                                      cameraMatrix,                                        //【4】摄像机内参数矩阵  
                      distCoeffs,                                          //【5】摄像机畸变向量(径向畸变,切向畸变k1,k2,k3,p1,p2)  
                          imagePoints2);                                       //【6】对于摄像机三维物理坐标框架下的位置,我们计算出来的该三维点在成像仪中的坐标(像素坐标)  
                                                                           
        err              = norm(Mat(imagePoints[i]), Mat(imagePoints2), CV_L2);//【7】两个数组对应元素差值平方的累加和  
        int n            = (int)objectPoints[i].size();                        //【8】Vector向量的成员函数--resize(),size(),push_back(),pop_back()                  
        perViewErrors[i] = (float)std::sqrt(err*err/n);                        //【9】单个三维点的投影误差  
  
        totalErr    += err*err;  
        totalPoints += n;  
    }  
  
    return std::sqrt(totalErr/totalPoints);                                    //【10】摄像机投影的总体误差  
}  
/****************************************************************************************************************************  
函数原型: 
       static void calcChessboardCorners(Size boardSize, float squareSize, vector<Point3f>& corners, Pattern patternType = CHESSBOARD) 
函数功能:  
       计算棋棋盘格----世界坐标系下----真实的物理三维坐标点的坐标 
函数参数:  
       1---Size boardSize-------------棋盘格的尺寸Size 
       2---float squareSize-----------棋盘格角点之间的距离Size 
       3---vector<Point3f>& corners---用来存储棋盘格角点的三维坐标 
       4---Pattern patternType--------标定板的类型 
函数返回值: 
       void 
****************************************************************************************************************************/    
static void calcChessboardCorners(Size boardSize, float squareSize, vector<Point3f>& corners, Pattern patternType = CHESSBOARD)  
{  
    corners.resize(0);                                               //【1】Vector向量的成员函数,重置Vector向量的长度为0  
  
    switch(patternType)                                              //【2】判断标定板的类型  
    {  
      case CHESSBOARD:                                               //【3】棋盘格类型的标定板  
      case CIRCLES_GRID:                                             //【4】圆心型风格的标定板  
        for( int i = 0; i < boardSize.height; i++ )  
            for( int j = 0; j < boardSize.width; j++ )  
                corners.push_back(Point3f(float(j*squareSize),       //【5】将摄像机坐标系下的三维世界实际的标定板的物理坐标存储在corners这个vector向量容器类  
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
函数原型: 
       static bool runCalibration(略） 
函数功能:  
       摄像机标定模块最核心的模块--------摄像机标定函数 
函数参数:  
       vector<vector<Point2f> > imagePoints,          //【1】输入控制变量---WCS下三维物理点的3D坐标数组,自己手动推导出来的三位坐标结果 
       Size                     imageSize,            //【2】输入控制变量---ICS坐标系下,根据findChessboardCorners()函数计算出来的标定图片上角点的坐标 
       Size                     boardSize,            //【3】输入控制变量---棋盘格的Size---棋盘格的横纵【内角点】个数 
       Pattern                  patternType,          //【4】输入控制变量---方块形棋盘格,原型型棋盘格,标定板模式 
       float                    squareSize,           //【5】输入控制变量---棋盘格角点之间的距离/圆心型标定板圆心之间的距离 
       float                    aspectRatio,          //【6】输入控制变量---长宽比 
       int                      flags,                //【7】输入控制变量---标志位 
       Mat&                     cameraMatrix,         //【1】待求解输出控制变量 ---摄像机内参数矩阵 
       Mat&                     distCoeffs,           //【2】待求解输出控制变量 ---摄像机畸变系数矩阵 
       vector<Mat>&             rvecs,                //【3】待求解输出控制变量---旋转矩阵 
       vector<Mat>&             tvecs,                //【4】待求解输出控制变量---畸变向量(k1,k2,k3,p1,p2) 
       vector<float>&           reprojErrs,           //【5】待求解输出控制变量---单幅图片/单个投影点的---投影误差/摄像机标定误差 
       double&                  totalAvgErr           //【6】待求解输出控制变量---摄像机标定的总体平均误差/投影平均误差 
函数返回值: 
       摄像机标定是否成功-----求解摄像机的内外参数矩阵是否成功 
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
    cameraMatrix = Mat::eye(3, 3, CV_64F);                                         //【1】摄像机内部参数矩阵,创建一个3*3的单位矩阵  
  
    if( flags & CV_CALIB_FIX_ASPECT_RATIO )  
        cameraMatrix.at<double>(0,0) = aspectRatio;  
  
    distCoeffs = Mat::zeros(8, 1, CV_64F);                                         //【2】摄像机的畸变系数向量,创建一个8*1的行向量  
  
    vector<vector<Point3f> > objectPoints(1);  
  
    calcChessboardCorners(boardSize, squareSize, objectPoints[0], patternType);    //【3】计算棋盘格角点世界坐标系下的三维物理坐标  
  
    objectPoints.resize(imagePoints.size(),objectPoints[0]);                       //【4】对objectPoints的Vector容器进行扩容,并且扩充的内存空间用元素objectPoints[0]填充  
                                                                                   //【5】摄像机标定函数----计算摄像机的内部参数和外部参数  
    double rms = calibrateCamera(objectPoints,      //【1】世界坐标系下*每张标定图片中的角点的总数k*图片的个数M---N*3矩阵(N=k*M)------物理坐标                                 
                                 imagePoints,       //【2】imagePoints是一个N*2的矩阵,它由objectPoints所提供的所有点所对应点的像素坐标构成,  
                                                          //如果使用棋盘格进行标定,那么，这个变量简单的由M次调用cvFindChessboardCorners()的返回值构成  
                                 imageSize,         //【3】imageSize是以像素衡量的图像的尺寸Size,图像点就是从该图像中提取的  
                                 cameraMatrix,      //【4】摄像机内部参数矩阵--------定义了理想摄像机的摄像机行为  
                                 distCoeffs,        //【5】畸变系数行向量5*1---8*1---更多的表征了摄像机的非理想行为  
                                 rvecs,             //【6】rotation_vectors----------旋转矩阵  
                                 tvecs,             //【7】tanslation_vectors--------平移矩阵  
                                 flags|CV_CALIB_FIX_K4|CV_CALIB_FIX_K5);          ///*|CV_CALIB_FIX_K3*/|CV_CALIB_FIX_K4|CV_CALIB_FIX_K5);  
  
    printf("RMS error reported by calibrateCamera: %g\n", rms);  
  
    bool ok = checkRange(cameraMatrix) && checkRange(distCoeffs);                  //【6】checkRange()函数---用于检查矩阵中的每一个元素是否在指定的一个数值区间之内  
  
    totalAvgErr = computeReprojectionErrors(objectPoints, imagePoints,             //【7】完成摄像机标定后，对标定进行评价，计算重投影误差/摄像机标定误差  
                rvecs, tvecs, cameraMatrix, distCoeffs, reprojErrs);               //【8】函数的返回值是摄像机标定/投影的总体平均误差  
  
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
函数原型:  
       static bool readStringList( const string& filename, vector<string>& l )  
函数功能:   
       读取字符串列表文件中的内容  
函数参数:   
       1---const string& filename-----要读取的XML文件的文件名  
       2---cvector<string>& l---------将文件中读取到的内容,存储在vector容器的对象l中  
函数返回值:  
       成功返回true  
****************************************************************************************************************************/    
static bool readStringList( const string& filename, vector<string>& l )  
{  
    l.resize(0);                                      //[1]重置向量的长度resize()  
    FileStorage fs(filename, FileStorage::READ);      //[2]使用OpenCv中的FileStorage文件存储类读取xml文件  
    if( !fs.isOpened() )                              //[3]判断文件是否已经打开,打开则为true  
        return false;  
    FileNode n = fs.getFirstTopLevelNode();           //[4]返回顶层映射的第一个节点,FileNode---文件节点类型  
    if( n.type() != FileNode::SEQ )                   //[5]文件节点的类型是不是序列Sequence--SEQ  
        return false;  
    FileNodeIterator it = n.begin(), it_end = n.end();//[6]遍历节点  
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
/*****************************************************【Main函数】************************************************************  
*      控制台应用程序的入口 
*****************************************************************************************************************************/  
int main( int argc, char** argv )  
{  
    Size boardSize;                                          //[1]标定板的Size  
    Size imageSize;                                          //[2]图片的Size  
  
    float squareSize  = 1.f;                                 //[3]棋盘格角点之间的距离  
    float aspectRatio = 1.f;                                 //[4]长宽比  
  
    Mat   cameraMatrix;                                      //[5]摄像机的内参数矩阵  
    Mat   distCoeffs;                                        //[6]摄像机的畸变系数向量  
    const char* outputFilename = "out_camera_data.yml";      //[7]输出的Xml文件名  
    const char* inputFilename  = 0;  
  
    int   i;  
    int   nframes           = 10;  
    bool  writeExtrinsics   = false;  
    bool  writePoints       = false;  
    bool  undistortImage    = false;  
    int   flags             = 0;  
  
  
    cv::VideoCapture        capture;                         //[8]定义一个视频类的类对象  
  
  
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
    //【模块1】如果没有任何参数的输入,则显示帮助信息  
    //=============================================================================================  
    if( argc < 2 )  
    {  
        help();  
        return 0;  
    }  
    //=============================================================================================  
    //【模块2】循环显示输入的参数,并判断每一个参数的类型  
    //=============================================================================================  
    for( i = 1; i < argc; i++ )  
    {  
        const char* s = argv[i];                                                            //[1]字符数组----存储---字符串  
        if( strcmp( s, "-w" ) == 0 )                                                        //[2]标定板的宽度  
        {  
            if( sscanf( argv[++i], "%u", &boardSize.width ) != 1 || boardSize.width <= 0 )  
                return fprintf( stderr, "Invalid board width\n" ), -1;                      //[3]这是个逗号表达式  
        }  
        else if( strcmp( s, "-h" ) == 0 )                                                   //[2]标定板的高度  
        {  
            if( sscanf( argv[++i], "%u", &boardSize.height ) != 1 || boardSize.height <= 0 )  
                return fprintf( stderr, "Invalid board height\n" ), -1;  
        }  
        else if( strcmp( s, "-pt" ) == 0 )                                                  //[3]棋盘格模式  
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
        else if( strcmp( s, "-s" ) == 0 )                                                  //[4]棋盘格角点之间的距离  
        {  
            if( sscanf( argv[++i], "%f", &squareSize ) != 1 || squareSize <= 0 )  
                return fprintf( stderr, "Invalid board square width\n" ), -1;  
        }  
        else if( strcmp( s, "-n" ) == 0 )                                                  //[5]图片数量  
        {  
            if( sscanf( argv[++i], "%u", &nframes ) != 1 || nframes <= 3 )  
                return printf("Invalid number of images\n" ), -1;  
        }  
        else if( strcmp( s, "-a" ) == 0 )                                                  //[6]长宽比  
        {  
            if( sscanf( argv[++i], "%f", &aspectRatio ) != 1 || aspectRatio <= 0 )  
                return printf("Invalid aspect ratio\n" ), -1;  
            flags |= CV_CALIB_FIX_ASPECT_RATIO;  
        }  
        else if( strcmp( s, "-d" ) == 0 )                                                  //[7]延迟时间  
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
    //【模块3】读取输入文件的文件名  
    //=============================================================================================  
    if( inputFilename )  
    {  
        if( !videofile && readStringList(inputFilename, imageList) )                       //[8]自定义函数readStringList()  
            mode = CAPTURING;  
        else  
            capture.open(inputFilename);  
    }  
    else  
        capture.open(cameraId);  
  
    if( !capture.isOpened() && imageList.empty() )                                         //[9]测试摄像头或者视频文件是否打开或者imagelist是否为空  
        return fprintf( stderr, "Could not initialize video (%d) capture\n",cameraId ), -2;  
  
    if( !imageList.empty() )  
        nframes = (int)imageList.size();                                                   //[10]返回vector向量元素的数目  
  
    if( capture.isOpened() )                                                               //[11]测试摄像头是否打开  
        printf( "%s", liveCaptureHelp );  
  
    namedWindow( "Image View", 1 );                                                        //[12]创建一个视频窗口  
  
    for(i = 0;;i++)  
    {  
        Mat view;  
        Mat viewGray;  
        bool blink = false;  
  
        if( capture.isOpened() )                                                           //[13]测试摄像头是否打开               
        {  
            Mat view0;  
            capture >> view0;  
            view0.copyTo(view);  
        }  
        else if( i < (int)imageList.size() )                                               //[14]读取图像序列中的而文件  
            view = imread(imageList[i], 1);  
    //=============================================================================================  
    //【模块4】进行摄像机标定,并且保存摄像机标定的结果  
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
        *使用图片序列的标定流程 
        **/  
        vector<Point2f> pointbuf;  
        cvtColor(view, viewGray, COLOR_BGR2GRAY);  
  
        bool found;  
          
        switch( pattern )                                                //【1】使用findChessboardCorners()函数寻找棋盘格标定板上角点的坐标信息---imagePoints  
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
  
        if( pattern == CHESSBOARD && found)                              //【2】使用cornerSubPix()函数将角点坐标精确到亚像素级别  
            cornerSubPix( viewGray, pointbuf, Size(11,11),Size(-1,-1), TermCriteria( CV_TERMCRIT_EPS+CV_TERMCRIT_ITER, 30, 0.1 ));  
  
        if( mode == CAPTURING && found &&(!capture.isOpened() || clock() - prevTimestamp > delay*1e-3*CLOCKS_PER_SEC) )  
        {  
            imagePoints.push_back(pointbuf);                            //【3】将缓存中的标定板上的角点坐标转存在imagePoints数组中  
            prevTimestamp = clock();  
            blink         = capture.isOpened();  
        }  
          
        if(found)                                                       //【4】使用drawChessboardCorners()绘制已经找出来的棋盘格角点  
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
  
        if( mode == CAPTURING && imagePoints.size() >= (unsigned)nframes ) //【5】开始进行摄像机标定  
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
                                                                           
    if( !capture.isOpened() && showUndistorted )                            //【6】进行图像校正  
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