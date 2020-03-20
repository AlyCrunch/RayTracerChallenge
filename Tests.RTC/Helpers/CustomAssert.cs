using RayTracerChallenge.Features;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xunit.Sdk;

namespace Tests.RTC.Helpers
{
    public class CustomAssert
    {
        public static void Equal(PointType expected, PointType actual, int precision)
        {
            if (!expected.Equals(actual,precision))
                throw new EqualException(
                    string.Format(CultureInfo.CurrentCulture, "Expected : {0}", expected.ToString("F" + precision)),
                    string.Format(CultureInfo.CurrentCulture, "Actual : {0}", actual.ToString("F" + precision))
                );
        }
        public static void Equal(Matrix expected, Matrix actual, int precision)
        {
            if (!expected.Equals(actual, precision))
                throw new EqualException(
                    string.Format(CultureInfo.CurrentCulture, "Expected : {0}", expected.ToString("F" + precision)),
                    string.Format(CultureInfo.CurrentCulture, "Actual : {0}", actual.ToString("F" + precision))
                );
        }
    }
}
