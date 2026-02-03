using Toast.Gamebase;

namespace Toast.Gamebase.Internal.Result
{
    public readonly struct GamebaseResult<T>
    {
        public T Value { get; }
        public readonly bool IsSuccess => Error is null;
        public GamebaseError Error { get; }

        public GamebaseResult(T result, GamebaseError error)
        {
            Value = result;
            Error = error;
        }

        public static GamebaseResult<T> Success(T value) => new GamebaseResult<T>(value, null);
        public static GamebaseResult<T> Failure(GamebaseError error) => new GamebaseResult<T>(default, error);
    }
}
