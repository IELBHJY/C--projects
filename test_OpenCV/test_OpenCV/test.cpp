#include<iostream>
#include<core/core.hpp>
#include<highgui/highgui.hpp>
using namespace cv;
using namespace std;
int main()  
{ 
//����ͼƬ��ע��ͼƬ·��  
Mat image=imread("C:\\Users\\lbh\\Desktop\\test.jpg");  
    
//ͼƬ����ɹ�����ж�  
if(!image.data)  
{  
  cout<<"you idiot��where did you hide james��"<<endl;    
//�ȴ�����  
system("pause");  
return -1;  
}  
//����һ������Ϊ��Lena����ͼ����ʾ���ڣ�������ǰ����Ҳ���ԣ�  
namedWindow("James",1);    
  
//��ʾͼ��  
imshow("James",image);    
  
//�ȴ�����  
waitKey();  
return 0;  
}  
