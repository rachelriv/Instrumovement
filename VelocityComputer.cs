﻿using System;
using Microsoft.Kinect;

namespace Instrumovement
{
    internal class VelocityComputer
    {
        
        public static double GetRelativeVelocity(JointType steady, JointType moving)
        {
            if (!(MainWindow.jointPositions.PositionExistsAt(steady, 0) && MainWindow.jointPositions.PositionExistsAt(steady, 1) &&
                MainWindow.jointPositions.PositionExistsAt(steady, 2) 
                && MainWindow.jointPositions.PositionExistsAt(moving, 0) && MainWindow.jointPositions.PositionExistsAt(moving, 1)
              && MainWindow.jointPositions.PositionExistsAt(moving, 2) ))
            {
                return 0;
            }

            CameraSpacePoint d0 = SubstractedPointsAt(steady, moving, 0);
            CameraSpacePoint d1 = SubstractedPointsAt(steady, moving, 1);

             if (!(MainWindow.jointPositions.PositionExistsAt(steady, 3) && MainWindow.jointPositions.PositionExistsAt(steady, 3)))
             {
                return DistanceBetweenPoints(d0, d1) / MainWindow.jointPositions.MillisBetweenPositions(moving, 1, 0);
             }
         
            CameraSpacePoint d2 = SubstractedPointsAt(steady, moving, 2);
            CameraSpacePoint d3 = SubstractedPointsAt(steady, moving, 3);

            return Median(
                DistanceBetweenPoints(d0, d1) * 1000.0 / MainWindow.jointPositions.MillisBetweenPositions(moving, 1, 0),
                DistanceBetweenPoints(d1, d2) * 1000.0 / MainWindow.jointPositions.MillisBetweenPositions(moving, 2, 1),
                DistanceBetweenPoints(d2, d3) * 1000.0 / MainWindow.jointPositions.MillisBetweenPositions(moving, 3, 2)
            );
        }

        public static double DistanceBetweenPoints(CameraSpacePoint point1, CameraSpacePoint point2)
        {
            double dx = Math.Abs(point2.X - point1.X);
            double dy = Math.Abs(point2.Y - point1.Y);

            double dz = Math.Abs(point2.Z - point1.Z);

            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }
        
        private static CameraSpacePoint SubstractedPointsAt(JointType steady, JointType moving, int n)
        {
            if (!(MainWindow.jointPositions.PositionExistsAt(steady, n) &&
                  MainWindow.jointPositions.PositionExistsAt(moving, n)))
            {
                return new CameraSpacePoint();
                //throw new ArgumentException("No Body Frame at this Index");
            }
            return SubstractPoints(MainWindow.jointPositions.GetNthMostRecentPosition(steady, n).position,
                                   MainWindow.jointPositions.GetNthMostRecentPosition(moving, n).position);
        }

        private static CameraSpacePoint SubstractPoints(CameraSpacePoint position1, CameraSpacePoint position2)
        {
            CameraSpacePoint result = new CameraSpacePoint();
            result.X = position1.X - position2.X;
            result.Y = position1.Y - position2.Y;
            result.Z = position1.Z - position2.Z;
            return result;

        }

        private static double Median(double d1, double d2, double d3)
        {
            if ((d1 > d2 && d1 < d3) || (d1 < d2 && d1 > d3))
            {
                return d1;
            }
            if ((d2 > d1 && d2 < d3) || (d2 < d1 && d2 > d3))
            {
                return d2;
            }
            return d3;
        }

    }
}