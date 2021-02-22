namespace additude.Tests
{
    /// <summary>
    /// Contains test result from the test excution
    /// </summary>
    public class Result
    {
        /// <value>The actual result, can be true or false.</value>
        public bool Value { get; set; }
        /// <value>Description of the result e.g. error message.</value>
        public string Message { get; set; } = "";

    }
}