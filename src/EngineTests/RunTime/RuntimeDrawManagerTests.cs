using Engine.RunTime.Managers;
using Engine.RunTime.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Moq;

namespace EngineTests.RunTime
{
    public class RuntimeDrawManagerTests
    {
        private class FakeDrawable : IAmDrawable
        {
            public int DrawLayer { get; set; }

            public void Draw(GameTime gameTime, GameServiceContainer gameServices) { }
        }

        private class SelfAddingDrawable : IAmDrawable
        {
            private RuntimeDrawManager Manager { get; init; }

            private IAmDrawable ToAdd { get; init; }

            public int DrawLayer { get; set; } = 1;

            public SelfAddingDrawable(RuntimeDrawManager manager, IAmDrawable toAdd)
            {
                Manager = manager;
                ToAdd = toAdd;
            }

            public void Draw(GameTime gt, GameServiceContainer services)
            {
                Manager.AddDrawable(ToAdd);
            }
        }

        private RuntimeDrawManager CreateManager(Game game = null)
        {
            game ??= new Game();
            var manager = new RuntimeDrawManager(game);
            manager.Initialize();

            return manager;
        }

        [Fact]
        public void AddDrawable_ShouldAddToActiveModels_WhenNoCurrentKey()
        {
            var manager = CreateManager();
            var drawable = new FakeDrawable { DrawLayer = 1 };

            Assert.False(manager.RunTimeCollection.ActiveModels.ContainsKey(1));

            manager.AddDrawable(drawable);

            Assert.True(manager.RunTimeCollection.ActiveModels.ContainsKey(1));
            Assert.Contains(drawable, manager.RunTimeCollection.ActiveModels[1]);
        }

        [Fact]
        public void AddDrawable_ShouldDeferToPendingAdds_WhenCurrentLayerIsActive()
        {
            var manager = CreateManager();
            var drawable = new FakeDrawable { DrawLayer = 1 };
            manager.AddDrawable(drawable);
            manager.RunTimeCollection.CurrentKey = 1;
            var newDrawable = new FakeDrawable { DrawLayer = 1 };
            manager.AddDrawable(newDrawable);

            Assert.DoesNotContain(newDrawable, manager.RunTimeCollection.ActiveModels[1]);
            Assert.Contains(newDrawable, manager.RunTimeCollection.PendingAdds);

            manager.RunTimeCollection.ResolvePendingModels();

            Assert.Contains(newDrawable, manager.RunTimeCollection.ActiveModels[1]);
        }

        [Fact]
        public void AddDrawable_ShouldDeferToPendingListAdds_WhenAddingNewLayerDuringActiveIterat()
        {
            var manager = CreateManager();
            var drawable = new FakeDrawable { DrawLayer = 2 };

            manager.RunTimeCollection.CurrentKey = 1;
            manager.AddDrawable(drawable);

            Assert.False(manager.RunTimeCollection.ActiveModels.ContainsKey(2));
            Assert.True(manager.RunTimeCollection.PendingListAdds.ContainsKey(2));
            Assert.Contains(drawable, manager.RunTimeCollection.PendingListAdds[2]);

            manager.RunTimeCollection.CurrentKey = null;
            manager.RunTimeCollection.ResolvePendingLists();

            Assert.True(manager.RunTimeCollection.ActiveModels.ContainsKey(2));
            Assert.Contains(drawable, manager.RunTimeCollection.ActiveModels[2]);
        }

        [Fact]
        public void RemoveDrawable_ShouldRemoveDrawableImmediately_WhenNoActiveIteration()
        {
            var manager = CreateManager();
            var drawable1 = new FakeDrawable { DrawLayer = 1 };
            var drawable2 = new FakeDrawable { DrawLayer = 1 };
            manager.AddDrawable(drawable1);
            manager.AddDrawable(drawable2);

            manager.RemoveDrawable(drawable1);

            Assert.False(true == manager.RunTimeCollection.ActiveModels[1].Contains(drawable1));
        }

        [Fact]
        public void RemoveDrawable_ShouldRemoveLayerEntry_WhenLastDrawableRemoved()
        {
            var manager = CreateManager();
            var drawable = new FakeDrawable { DrawLayer = 1 };
            manager.AddDrawable(drawable);

            Assert.True(manager.RunTimeCollection.ActiveModels.ContainsKey(1));

            manager.RemoveDrawable(drawable);

            Assert.False(manager.RunTimeCollection.ActiveModels.ContainsKey(1));
        }

        [Fact]
        public void RemoveDrawable_ShouldDeferToPendingRemovals_WhenRemovingDuringActiveIteration()
        {
            var manager = CreateManager();
            var drawable = new FakeDrawable { DrawLayer = 1 };

            manager.AddDrawable(drawable);
            manager.RunTimeCollection.CurrentKey = 1;
            manager.RemoveDrawable(drawable);

            Assert.Contains(drawable, manager.RunTimeCollection.ActiveModels[1]);
            Assert.Contains(drawable, manager.RunTimeCollection.PendingRemovals);

            manager.RunTimeCollection.ResolvePendingModels();

            Assert.DoesNotContain(drawable, manager.RunTimeCollection.ActiveModels[1]);
        }

        [Fact]
        public void RemoveDrawable_ShouldQueueAndApplyListRemoval_WhenOutsideActiveIteration()
        {
            var manager = CreateManager();
            var drawable = new FakeDrawable { DrawLayer = 1 };

            manager.AddDrawable(drawable);
            manager.RunTimeCollection.CurrentKey = 2;
            manager.RemoveDrawable(drawable);

            Assert.Contains(1, manager.RunTimeCollection.PendingListRemovals);
            Assert.True(manager.RunTimeCollection.ActiveModels.ContainsKey(1));
            Assert.Empty(manager.RunTimeCollection.ActiveModels[1]);

            manager.RunTimeCollection.CurrentKey = null;
            manager.RunTimeCollection.ResolvePendingLists();

            Assert.False(manager.RunTimeCollection.ActiveModels.ContainsKey(1));
        }

        [Fact]
        public void ChangeDrawableLayer_ShouldMoveImmediately_WhenNoActiveIteration()
        {
            var manager = CreateManager();
            var drawable = new FakeDrawable { DrawLayer = 1 };
            manager.AddDrawable(drawable);

            Assert.True(manager.RunTimeCollection.ActiveModels.ContainsKey(1));
            Assert.False(manager.RunTimeCollection.ActiveModels.ContainsKey(2));

            manager.ChangeDrawableLayer(2, drawable);

            Assert.False(manager.RunTimeCollection.ActiveModels.ContainsKey(1));
            Assert.True(manager.RunTimeCollection.ActiveModels.ContainsKey(2));
            Assert.Equal(2, drawable.DrawLayer);
        }

        [Fact]
        public void ChangeDrawableLayer_ShouldDeferMove_WhenCalledDuringActiveIteration()
        {
            var manager = CreateManager();
            var drawable = new FakeDrawable { DrawLayer = 1 };

            manager.AddDrawable(drawable);
            manager.RunTimeCollection.CurrentKey = 1;
            manager.ChangeDrawableLayer(2, drawable);

            Assert.True(manager.RunTimeCollection.PendingListAdds.ContainsKey(2));
            Assert.Contains(drawable, manager.RunTimeCollection.PendingRemovals);

            Assert.True(manager.RunTimeCollection.ActiveModels.ContainsKey(1));
            Assert.False(manager.RunTimeCollection.ActiveModels.ContainsKey(2));

            manager.RunTimeCollection.ResolvePendingModels();
            manager.RunTimeCollection.CurrentKey = null;
            manager.RunTimeCollection.ResolvePendingLists();

            Assert.False(manager.RunTimeCollection.ActiveModels.ContainsKey(1));
            Assert.True(manager.RunTimeCollection.ActiveModels.ContainsKey(2));
            Assert.Equal(2, drawable.DrawLayer);
        }

        [Fact]
        public void Draw_ShouldCallDrawingServiceAndEachDrawable()
        {
            var game = new Game();
            var drawingServiceMock = new Mock<IDrawingService>();
            game.Services.AddService(typeof(IDrawingService), drawingServiceMock.Object);
            var manager = CreateManager(game);

            var drawableMock = new Mock<IAmDrawable>();
            drawableMock.SetupGet(d => d.DrawLayer).Returns(1);

            manager.AddDrawable(drawableMock.Object);

            manager.Draw(new GameTime());

            drawingServiceMock.Verify(d => d.BeginDraw(), Times.Once);
            drawingServiceMock.Verify(d => d.EndDraw(), Times.Once);
            drawableMock.Verify(d => d.Draw(It.IsAny<GameTime>(), game.Services), Times.Once);
        }

        [Fact]
        public void Draw_ShouldInvokeEachDrawableExactlyOncePerFrame()
        {
            var game = new Game();
            var drawingServiceMock = new Mock<IDrawingService>();
            game.Services.AddService(typeof(IDrawingService), drawingServiceMock.Object);

            var manager = CreateManager(game);

            var drawableMocks = new List<Mock<IAmDrawable>>();

            for (int i = 0; i < 10; i++)
            {
                var drawableMock = new Mock<IAmDrawable>();
                drawableMock.SetupGet(d => d.DrawLayer).Returns(1);
                drawableMocks.Add(drawableMock);

                manager.AddDrawable(drawableMock.Object);
            }

            manager.Draw(new GameTime());

            drawingServiceMock.Verify(d => d.BeginDraw(), Times.Once);
            drawingServiceMock.Verify(d => d.EndDraw(), Times.Once);

            foreach (var mock in drawableMocks)
            {
                mock.Verify(d => d.Draw(It.IsAny<GameTime>(), game.Services), Times.Once);
            }
        }

        [Fact]
        public void Draw_ShouldDeferNewLayerUntilNextFrame_WhenAddedMidFrame()
        {
            var game = new Game();
            var drawingServiceMock = new Mock<IDrawingService>();
            game.Services.AddService(typeof(IDrawingService), drawingServiceMock.Object);

            var manager = CreateManager(game);

            var newDrawableMock = new Mock<IAmDrawable>();
            newDrawableMock.SetupGet(d => d.DrawLayer).Returns(2);

            var addingDrawable = new SelfAddingDrawable(manager, newDrawableMock.Object) { DrawLayer = 1 };

            manager.AddDrawable(addingDrawable);

            manager.Draw(new GameTime());
            newDrawableMock.Verify(d => d.Draw(It.IsAny<GameTime>(), game.Services), Times.Never);

            manager.Draw(new GameTime());
            newDrawableMock.Verify(d => d.Draw(It.IsAny<GameTime>(), game.Services), Times.Once);
        }

        [Fact]
        public void Draw_ShouldRespectLayerOrdering_WhenDrawing()
        {
            var game = new Game();
            var drawingServiceMock = new Mock<IDrawingService>();
            game.Services.AddService(typeof(IDrawingService), drawingServiceMock.Object);
            var manager = CreateManager(game);

            var callOrder = new List<int>();

            var drawable1 = new Mock<IAmDrawable>();
            drawable1.SetupGet(d => d.DrawLayer).Returns(1);
            drawable1.Setup(d => d.Draw(It.IsAny<GameTime>(), game.Services))
                     .Callback(() => callOrder.Add(1));

            var drawable2 = new Mock<IAmDrawable>();
            drawable2.SetupGet(d => d.DrawLayer).Returns(2);
            drawable2.Setup(d => d.Draw(It.IsAny<GameTime>(), game.Services))
                     .Callback(() => callOrder.Add(2));

            manager.AddDrawable(drawable2.Object);
            manager.AddDrawable(drawable1.Object);

            manager.Draw(new GameTime());

            Assert.Equal(new[] { 1, 2 }, callOrder);
        }

        [Fact]
        public void Draw_ShouldBeginAndEndEvenWhenNoDrawablesPresent()
        {
            var game = new Game();
            var drawingServiceMock = new Mock<IDrawingService>();
            game.Services.AddService(typeof(IDrawingService), drawingServiceMock.Object);
            var manager = CreateManager(game);

            manager.Draw(new GameTime());

            drawingServiceMock.Verify(d => d.BeginDraw(), Times.Once);
            drawingServiceMock.Verify(d => d.EndDraw(), Times.Once);
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