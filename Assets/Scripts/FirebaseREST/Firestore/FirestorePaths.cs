using System;

public static class FirestorePaths
{
    // 🔹 Returns "users/{uid}/progress/{yyyy-MM-dd}"
    public static string UserProgress(string userId, DateTime date)
    {
        return $"users/{userId}/progress/{date:yyyy-MM-dd}";
    }

    // 🔹 Returns "users/{uid}/plans/{planId}"
    public static string UserPlan(string userId, string planId)
    {
        return $"users/{userId}/plans/{planId}";
    }

    // 🔹 Returns "users/{uid}/games/{gameId}"
    public static string UserGameScore(string userId, string gameId)
    {
        return $"users/{userId}/games/{gameId}";
    }

    // 🔹 Returns "users/{uid}/answers/{quizId}"
    public static string UserAnswer(string userId, string quizId)
    {
        return $"users/{userId}/answers/{quizId}";
    }

    // 🔹 Gets only collection path, e.g. "users/{uid}/progress"
    public static string UserProgressCollection(string userId)
    {
        return $"users/{userId}/progress";
    }
}
