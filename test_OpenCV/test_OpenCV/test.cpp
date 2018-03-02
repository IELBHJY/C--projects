#include<iostream>
#include<core/core.hpp>
#include<highgui/highgui.hpp>
using namespace cv;
using namespace std;
int main()  
{ 
//读入图片，注意图片路径  
Mat image=imread("C:\\Users\\lbh\\Desktop\\test.jpg");  
    
//图片读入成功与否判定  
if(!image.data)  
{  
  cout<<"you idiot！where did you hide james！"<<endl;    
//等待按键  
system("pause");  
return -1;  
}  
//创建一个名字为“Lena”的图像显示窗口，（不提前声明也可以）  
namedWindow("James",1);    
  
//显示图像  
imshow("James",image);    
  
//等待按键  
waitKey();  
return 0;  
}  
