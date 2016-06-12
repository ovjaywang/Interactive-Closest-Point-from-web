/*
Copyright 2011. All rights reserved.
Institute of Measurement and Control Systems
Karlsruhe Institute of Technology, Germany

This file is part of libicp.
Authors: Andreas Geiger

libicp is free software; you can redistribute it and/or modify it under the
terms of the GNU General Public License as published by the Free Software
Foundation; either version 3 of the License, or any later version.

libicp is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
PARTICULAR PURPOSE. See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with
libicp; if not, write to the Free Software Foundation, Inc., 51 Franklin
Street, Fifth Floor, Boston, MA 02110-1301, USA 
*/

// Demo program showing how libicp can be used

#include <iostream>
#include "icpPointToPoint.h"
#include "icpPointToPlane.h"
#include "lasreader.h"
#include "laswriter.h"
#include "lasdefinitions.h"
#include<stdio.h>
#include<stdlib.h>
#include<time.h>
#define random(x) (rand()%x)
using namespace std;

int main (int argc, char** argv) {

  // define a 3 dim problem with 10000 model points
  // and 10000 template points:
  
		srand((int)time(0));
		for (int x = 0; x < 10; x++)
		{
			
		}
	
  int32_t dim = 3;
  
  int32_t num=10000;
  double* M = new double[3*num];//模型点source
  double* T =  new double[3*num];//实例点 template
  
  // set model and template points
  cout << endl << "Creating model with 10000 points ..." << endl;
  cout << "Creating template by shifting model by (1,1,1) ..." << endl;
  int32_t k=0;
  //for (double x=-2; x<2; x+=0.04) {
  //  for (double y=-2; y<2; y+=0.04) {
  //    double z=5*x*exp(-x*x-y*y);
  //    M[k*3+0] = x;
  //    M[k*3+1] = y;
  //    M[k*3+2] = z;
  //    T[k*3+0] = x-1;
  //    T[k*3+1] = y-1;
  //    T[k*3+2] = z-1;
  //    k++;
  //  }
  //}
  for (double x = -2; x<2; x += 0.04) {
	  for (double y = -2; y<2; y += 0.04) {
		  double z = 5 * x*exp(-x*x - y*y);
		  M[k * 3 + 0] = y;
		  M[k * 3 + 1] = z;
		  M[k * 3 + 2] = x;
		  float aa = random(50)*1.0f;
		  //printf("%f ", aa / 200.0f);
		  T[k * 3 + 0] = x - 1-aa/300.0f;
		  T[k * 3 + 1] = y - 1+aa/400.0f;
		  T[k * 3 + 2] = z - 1+aa/200.0f;
		  k++;
	  }
  }
  // start with identity as initial transformation
  // in practice you might want to use some kind of prediction here
  Matrix R = Matrix::eye(3);//单位阵E
  Matrix t(3,1);//平移矩阵 3*1
  //double m=0.01;

  // run point-to-plane ICP (-1 = no outlier threshold)
  cout << endl << "Running ICP (point-to-plane, no outliers)" << endl;
  IcpPointToPlane icp(M,num,dim);
  icp.fit(T,num,R,t,-1);

  // results
  cout << endl << "Transformation results:" << endl;
  cout << "R:" << endl << R << endl << endl;
  cout << "t:" << endl << t << endl << endl;
  system("pause");
  // free memory
  free(M);
 free(T);
  //delete []M;
   //delete []T;

  // success
  return 0;
}

