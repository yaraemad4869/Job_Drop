using Xunit;
using FluentAssertions;
using JobDrop.Domain.Common;

namespace JobDrop.Domain.UnitTests.Entities;

public class TestEntity : BaseEntity { }

public class BaseEntityTests
{
    [Fact]
    public void Constructor_ShouldGenerateId()
    {
        var entity = new TestEntity();
        entity.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Equals_SameId_ShouldBeEqual()
    {
        var entity1 = new TestEntity();
        var entity2 = entity1;
        entity1.Should().Be(entity2);
    }

    [Fact]
    public void Equals_DifferentId_ShouldNotBeEqual()
    {
        var entity1 = new TestEntity();
        var entity2 = new TestEntity();
        entity1.Should().NotBe(entity2);
    }

    [Fact]
    public void AddDomainEvent_ShouldAddEvent()
    {
        var entity = new TestEntity();
        var domainEvent = new TestDomainEvent();
        entity.AddDomainEvent(domainEvent);
        entity.DomainEvents.Should().Contain(domainEvent);
    }

    [Fact]
    public void ClearDomainEvents_ShouldRemoveAllEvents()
    {
        var entity = new TestEntity();
        entity.AddDomainEvent(new TestDomainEvent());
        entity.AddDomainEvent(new TestDomainEvent());
        entity.ClearDomainEvents();
        entity.DomainEvents.Should().BeEmpty();
    }

    private record TestDomainEvent : DomainEvent { }
}