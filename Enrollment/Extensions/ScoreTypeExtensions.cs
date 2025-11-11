using Enrollment.Models;

namespace Enrollment.Extensions;

public static class ScoreTypeExtensions
{
    public static double GetDigit(this ScoreType scoreType)
    {
        return scoreType switch
        {
            ScoreType.A_PLUS => 4.5,
            ScoreType.A_ZERO => 4.0,
            ScoreType.B_PLUS => 3.5,
            ScoreType.B_ZERO => 3.0,
            ScoreType.C_PLUS => 2.5,
            ScoreType.C_ZERO => 2.0,
            ScoreType.D_PLUS => 1.5,
            ScoreType.D_ZERO => 1.0,
            ScoreType.F => 0.0,
            ScoreType.PASS => 4.5,
            ScoreType.NO_PASS => 0.0,
            _ => 0.0
        };
    }
}