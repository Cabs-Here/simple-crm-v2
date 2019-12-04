using System.Collections.Generic;

namespace Classroom.SimpleCRM
{
    /// <summary>
    /// A container for validation errors used in business layer logic.
    /// Useful to return results from a workflow or errors based on status of back-end data
    /// or user record ownership.
    /// </summary>
    public class ValidationState
    {
        public List<ValidationError> Errors { get; set; }
        public ValidationState()
        {
            Errors = new List<ValidationError>();
        }
    }
}