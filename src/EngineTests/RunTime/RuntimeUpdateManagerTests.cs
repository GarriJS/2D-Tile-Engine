using Engine.RunTime.Managers;
using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework;
using Moq;

namespace EngineTests.RunTime
{
	public class RuntimeUpdateManagerTests
    {
        private class FakeUpdateable : IAmUpdateable
		{
            public int UpdateOrder { get; set; }

            public void Update(GameTime gameTime, GameServiceContainer gameServices) { }
        }

        private class SelfAddingUpdateable : IAmUpdateable
		{
            private RuntimeUpdateManager Manager { get; init; }

            private IAmUpdateable ToAdd { get; init; }

            public int UpdateOrder { get; set; } = 1;

			public SelfAddingUpdateable(RuntimeUpdateManager manager, IAmUpdateable toAdd)
            {
                Manager = manager;
                ToAdd = toAdd;
            }

            public void Update(GameTime gt, GameServiceContainer services)
            {
                Manager.AddUpdateable(ToAdd);
            }
        }

        private RuntimeUpdateManager CreateManager(Game game = null)
        {
            game ??= new Game();
            var manager = new RuntimeUpdateManager(game);
            manager.Initialize();

            return manager;
        }

        [Fact]
        public void AddUpdateable_ShouldAddToActiveModels_WhenNoCurrentKey()
        {
            var manager = CreateManager();
            var drawable = new FakeUpdateable { UpdateOrder = 1 };

            Assert.False(manager.RunTimeCollection.ActiveModels.ContainsKey(1));

            manager.AddUpdateable(drawable);

            Assert.True(manager.RunTimeCollection.ActiveModels.ContainsKey(1));
            Assert.Contains(drawable, manager.RunTimeCollection.ActiveModels[1]);
        }

        [Fact]
        public void AddUpdateable_ShouldDeferToPendingAdds_WhenCurrentOrderIsActive()
        {
            var manager = CreateManager();
            var drawable = new FakeUpdateable { UpdateOrder = 1 };
            manager.AddUpdateable(drawable);
            manager.RunTimeCollection.CurrentKey = 1;
            var newUpdateable = new FakeUpdateable { UpdateOrder = 1 };
            manager.AddUpdateable(newUpdateable);

            Assert.DoesNotContain(newUpdateable, manager.RunTimeCollection.ActiveModels[1]);
            Assert.Contains(newUpdateable, manager.RunTimeCollection.PendingAdds);

            manager.RunTimeCollection.ResolvePendingModels();

            Assert.Contains(newUpdateable, manager.RunTimeCollection.ActiveModels[1]);
        }

        [Fact]
        public void AddUpdateable_ShouldDeferToPendingListAdds_WhenAddingNewOrderDuringActiveIterate()
        {
            var manager = CreateManager();
            var drawable = new FakeUpdateable { UpdateOrder = 2 };

            manager.RunTimeCollection.CurrentKey = 1;
            manager.AddUpdateable(drawable);

            Assert.False(manager.RunTimeCollection.ActiveModels.ContainsKey(2));
            Assert.True(manager.RunTimeCollection.PendingListAdds.ContainsKey(2));
            Assert.Contains(drawable, manager.RunTimeCollection.PendingListAdds[2]);

            manager.RunTimeCollection.CurrentKey = null;
            manager.RunTimeCollection.ResolvePendingLists();

            Assert.True(manager.RunTimeCollection.ActiveModels.ContainsKey(2));
            Assert.Contains(drawable, manager.RunTimeCollection.ActiveModels[2]);
        }

        [Fact]
        public void RemoveUpdateable_ShouldRemoveUpdateableImmediately_WhenNoActiveIteration()
        {
            var manager = CreateManager();
            var drawable1 = new FakeUpdateable { UpdateOrder = 1 };
            var drawable2 = new FakeUpdateable { UpdateOrder = 1 };
            manager.AddUpdateable(drawable1);
            manager.AddUpdateable(drawable2);

            manager.RemoveUpdateable(drawable1);

            Assert.False(true == manager.RunTimeCollection.ActiveModels[1].Contains(drawable1));
        }

        [Fact]
        public void RemoveUpdateable_ShouldRemoveOrderEntry_WhenLastUpdateableRemoved()
        {
            var manager = CreateManager();
            var drawable = new FakeUpdateable { UpdateOrder = 1 };
            manager.AddUpdateable(drawable);

            Assert.True(manager.RunTimeCollection.ActiveModels.ContainsKey(1));

            manager.RemoveUpdateable(drawable);

            Assert.False(manager.RunTimeCollection.ActiveModels.ContainsKey(1));
        }

        [Fact]
        public void RemoveUpdateable_ShouldDeferToPendingRemovals_WhenRemovingDuringActiveIteration()
        {
            var manager = CreateManager();
            var drawable = new FakeUpdateable { UpdateOrder = 1 };

            manager.AddUpdateable(drawable);
            manager.RunTimeCollection.CurrentKey = 1;
            manager.RemoveUpdateable(drawable);

            Assert.Contains(drawable, manager.RunTimeCollection.ActiveModels[1]);
            Assert.Contains(drawable, manager.RunTimeCollection.PendingRemovals);

            manager.RunTimeCollection.ResolvePendingModels();

            Assert.DoesNotContain(drawable, manager.RunTimeCollection.ActiveModels[1]);
        }

        [Fact]
        public void RemoveUpdateable_ShouldQueueAndApplyListRemoval_WhenOutsideActiveIteration()
        {
            var manager = CreateManager();
            var drawable = new FakeUpdateable { UpdateOrder = 1 };

            manager.AddUpdateable(drawable);
            manager.RunTimeCollection.CurrentKey = 2;
            manager.RemoveUpdateable(drawable);

            Assert.Contains(1, manager.RunTimeCollection.PendingListRemovals);
            Assert.True(manager.RunTimeCollection.ActiveModels.ContainsKey(1));
            Assert.Empty(manager.RunTimeCollection.ActiveModels[1]);

            manager.RunTimeCollection.CurrentKey = null;
            manager.RunTimeCollection.ResolvePendingLists();

            Assert.False(manager.RunTimeCollection.ActiveModels.ContainsKey(1));
        }

        [Fact]
        public void ChangeUpdateableOrder_ShouldMoveImmediately_WhenNoActiveIteration()
        {
            var manager = CreateManager();
            var drawable = new FakeUpdateable { UpdateOrder = 1 };
            manager.AddUpdateable(drawable);

            Assert.True(manager.RunTimeCollection.ActiveModels.ContainsKey(1));
            Assert.False(manager.RunTimeCollection.ActiveModels.ContainsKey(2));

            manager.ChangeUpdateableOrder(2, drawable);

            Assert.False(manager.RunTimeCollection.ActiveModels.ContainsKey(1));
            Assert.True(manager.RunTimeCollection.ActiveModels.ContainsKey(2));
            Assert.Equal(2, drawable.UpdateOrder);
        }

        [Fact]
        public void ChangeUpdateableOrder_ShouldDeferMove_WhenCalledDuringActiveIteration()
        {
            var manager = CreateManager();
            var drawable = new FakeUpdateable { UpdateOrder = 1 };

            manager.AddUpdateable(drawable);
            manager.RunTimeCollection.CurrentKey = 1;
            manager.ChangeUpdateableOrder(2, drawable);

            Assert.True(manager.RunTimeCollection.PendingListAdds.ContainsKey(2));
            Assert.Contains(drawable, manager.RunTimeCollection.PendingRemovals);

            Assert.True(manager.RunTimeCollection.ActiveModels.ContainsKey(1));
            Assert.False(manager.RunTimeCollection.ActiveModels.ContainsKey(2));

            manager.RunTimeCollection.ResolvePendingModels();
            manager.RunTimeCollection.CurrentKey = null;
            manager.RunTimeCollection.ResolvePendingLists();

            Assert.False(manager.RunTimeCollection.ActiveModels.ContainsKey(1));
            Assert.True(manager.RunTimeCollection.ActiveModels.ContainsKey(2));
            Assert.Equal(2, drawable.UpdateOrder);
        }

        [Fact]
        public void Update_ShouldCallUpdateable()
        {
            var game = new Game();
            var manager = CreateManager(game);

            var updateableMock = new Mock<IAmUpdateable>();
            updateableMock.SetupGet(d => d.UpdateOrder).Returns(1);

            manager.AddUpdateable(updateableMock.Object);
            manager.Update(new GameTime());

            updateableMock.Verify(d => d.Update(It.IsAny<GameTime>(), game.Services), Times.Once);
        }

        [Fact]
        public void Update_ShouldInvokeEachUpdateableExactlyOncePerFrame()
        {
            var game = new Game();

            var manager = CreateManager(game);

            var updateableMocks = new List<Mock<IAmUpdateable>>();

            for (int i = 0; i < 10; i++)
            {
                var updateableMock = new Mock<IAmUpdateable>();
                updateableMock.SetupGet(d => d.UpdateOrder).Returns(1);
                updateableMocks.Add(updateableMock);

                manager.AddUpdateable(updateableMock.Object);
            }

            manager.Update(new GameTime());

            foreach (var mock in updateableMocks)
            {
                mock.Verify(d => d.Update(It.IsAny<GameTime>(), game.Services), Times.Once);
            }
        }

        [Fact]
        public void Update_ShouldDeferNewOrderUntilNextFrame_WhenAddedMidFrame()
        {
            var game = new Game();

            var manager = CreateManager(game);

            var newUpdateableMock = new Mock<IAmUpdateable>();
            newUpdateableMock.SetupGet(d => d.UpdateOrder).Returns(2);

            var addingUpdateable = new SelfAddingUpdateable(manager, newUpdateableMock.Object) { UpdateOrder = 1 };

            manager.AddUpdateable(addingUpdateable);

            manager.Update(new GameTime());
            newUpdateableMock.Verify(d => d.Update(It.IsAny<GameTime>(), game.Services), Times.Never);

            manager.Update(new GameTime());
            newUpdateableMock.Verify(d => d.Update(It.IsAny<GameTime>(), game.Services), Times.Once);
        }

        [Fact]
        public void Update_ShouldRespectOrderOrdering_WhenUpdating()
        {
            var game = new Game();
            var manager = CreateManager(game);

            var callOrder = new List<int>();

            var drawable1 = new Mock<IAmUpdateable>();
            drawable1.SetupGet(d => d.UpdateOrder).Returns(1);
            drawable1.Setup(d => d.Update(It.IsAny<GameTime>(), game.Services))
                     .Callback(() => callOrder.Add(1));

            var drawable2 = new Mock<IAmUpdateable>();
            drawable2.SetupGet(d => d.UpdateOrder).Returns(2);
            drawable2.Setup(d => d.Update(It.IsAny<GameTime>(), game.Services))
                     .Callback(() => callOrder.Add(2));

            manager.AddUpdateable(drawable2.Object);
            manager.AddUpdateable(drawable1.Object);

            manager.Update(new GameTime());

            Assert.Equal(new[] { 1, 2 }, callOrder);
        }

        // Behavior subject to change.
        [Fact]
        public void ResolvePendingLists_ShouldDiscardOperations_WhenActiveIterationInProgress()
        {
            var manager = CreateManager();

            manager.RunTimeCollection.PendingListRemovals.Add(123);
            manager.RunTimeCollection.CurrentKey = 1;

            manager.RunTimeCollection.ResolvePendingLists();

            Assert.Empty(manager.RunTimeCollection.PendingListRemovals);
        }
    }
}