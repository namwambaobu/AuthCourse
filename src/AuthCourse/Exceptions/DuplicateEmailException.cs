namespace AuthCourse.Exceptions;

public sealed class DuplicateEmailException(string email)
    : Exception($"A user with email '{email}' already exists.");