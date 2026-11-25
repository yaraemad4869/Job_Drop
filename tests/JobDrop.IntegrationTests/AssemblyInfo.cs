using Xunit;

// Disable parallelization to prevent database drop/create conflicts between tests
[assembly: CollectionBehavior(DisableTestParallelization = true)]