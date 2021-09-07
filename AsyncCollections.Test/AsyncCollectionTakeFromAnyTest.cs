﻿using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace HellBrick.Collections.Test
{
	public class AsyncCollectionTakeFromAnyTest
	{
		private readonly AsyncCollection<int>[] _collections;

		public AsyncCollectionTakeFromAnyTest() => _collections = new AsyncCollection<int>[ 2 ] { new AsyncCollection<int>( new ConcurrentQueue<int>() ), new AsyncCollection<int>( new ConcurrentQueue<int>() ) };

		[Fact]
		public async Task ReturnsItemFromSecondIfFirstIsEmpty()
		{
			_collections[ 1 ].Add( 42 );

			AnyResult<int> result = await AsyncCollection<int>.TakeFromAnyAsync( _collections ).ConfigureAwait( true );
			result.Value.Should().Be( 42 );
			result.CollectionIndex.Should().Be( 1 );
		}

		[Fact]
		public async Task NoUnnecessaryAwaitersAreQueued()
		{
			_collections[ 1 ].Add( 42 );

			AnyResult<int> result = await AsyncCollection<int>.TakeFromAnyAsync( _collections ).ConfigureAwait( true );
			_collections[ 0 ].AwaiterCount.Should().Be( 0 );
		}

		[Fact]
		public async Task RespectsCollectionOrder()
		{
			_collections[ 0 ].Add( 42 );
			_collections[ 1 ].Add( 24 );

			AnyResult<int> result = await AsyncCollection<int>.TakeFromAnyAsync( _collections ).ConfigureAwait( true );
			result.Value.Should().Be( 42 );
			result.CollectionIndex.Should().Be( 0 );
		}

		[Fact]
		public async Task ReturnsItemIfItIsAddedLater()
		{
			ValueTask<AnyResult<int>> task = AsyncCollection<int>.TakeFromAnyAsync( _collections );
			task.IsCompleted.Should().BeFalse();

			_collections[ 1 ].Add( 42 );
			AnyResult<int> result = await task.ConfigureAwait( true );
			result.Value.Should().Be( 42 );
			result.CollectionIndex.Should().Be( 1 );
		}

/*		[Fact]
		public void CancelsTaskWhenTokenIsCanceled()
		{
			CancellationTokenSource cancelSource = new CancellationTokenSource();
			ValueTask<AnyResult<int>> task = AsyncCollection<int>.TakeFromAnyAsync( _collections, cancelSource.Token );

			cancelSource.Cancel();
			Func<Task> asyncAct = async () => await task.ConfigureAwait( false );
			asyncAct.ShouldThrow<TaskCanceledException>();

			_collections[ 0 ].Add( 42 );
			_collections[ 1 ].Add( 64 );
			_collections[ 0 ].Count.Should().Be( 1 );
			_collections[ 1 ].Count.Should().Be( 1 );
		}*/

		[Fact]
		public void DoesNothingIfTokenIsCanceledBeforeMethodCall()
		{
			CancellationTokenSource cancelSource = new CancellationTokenSource();
			cancelSource.Cancel();
			_collections[ 0 ].Add( 42 );

			ValueTask<AnyResult<int>> task = AsyncCollection<int>.TakeFromAnyAsync( _collections, cancelSource.Token );

			task.IsCanceled.Should().BeTrue();
			_collections[ 0 ].Count.Should().Be( 1 );
		}
	}
}
