#ifndef _ICP_H_
#define _ICP_H_

#include "cv.h"
#include "highgui.h"
#include <vector>
using namespace std;

typedef unsigned char uchar;

typedef struct _Point3D
{
	double x, y, z;
	uchar r, g, b;
}Point3D;

void ReadPoint3D(const char *filename, vector<Point3D> &P);
void SelectPoint3D(const char *filename, vector<Point3D> &P, int ratio = 100);
void WritePLY(const char *filename, vector<Point3D> &cloud);

void ICP(vector<Point3D> &model, vector<Point3D> &data, double *R, double *T, double e = 0.1);

void FindClosestPointSet(vector<Point3D> &model, vector<Point3D> &data, vector<Point3D> &Y);

void CalculateMeanPoint3D(vector<Point3D> &P, Point3D &mean);

void MatrixMul(double *p, double *y, double *mul, int m1, int n1, int m2, int n2);
void MatrixAdd(double *a, double *b, int m, int n);
void MatrixDiv(double *a, double *b, int m, int n);
void MatrixTran(double *src, double *src_T, int m, int n);
double MatrixTR(double *m, int n);
void MatrixEigen(double *m, double *eigen, double *q, int n);
void PrintMatrix(double *M, int m, int n);

void CalculateRotation(double *q, double *R);

void TransPoint(vector<Point3D> &src, vector<Point3D> &dst, double *R, double *T);

#endif