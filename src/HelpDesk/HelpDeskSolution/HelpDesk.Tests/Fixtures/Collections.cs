namespace HelpDesk.Tests.Fixtures;


[CollectionDefinition("AuthenticatedSystemTestFixture")]
public class SystemTestFixtureCollection : ICollectionFixture<AuthenticatedSystemTestFixture>;


[CollectionDefinition("AnonymousSystemTestFixture")]
public class AnonymousTestFixtureCollection : ICollectionFixture<AnonymousSystemTestFixture>;