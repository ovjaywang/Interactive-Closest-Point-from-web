#include "cv.h"
#include "highgui.h"

#include "ICP.h"
#include "Match3D.h"

void testICP()
{
	vector<Point3D> model, data;
	double R[9], T[3], e = 0.001;

	ICP(model, data, R, T, e);
}

int main()
{
	testICP();

	return 0;
}
