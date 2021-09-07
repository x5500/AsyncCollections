// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;

#pragma warning disable OverrideGetHashCode // Structs should override GetHashCode()
#pragma warning disable ImplementOperatorEqualsEqualsToken // Structs should implement operator ==
#pragma warning disable ImplementOperatorExclamationEqualsToken // Structs should implement operator !=
#pragma warning disable HBStructImplementIEquatable // Structs should implement IEquatable<T>
#pragma warning disable OverrideEquals // Structs should override Equals()

namespace System.Runtime.CompilerServices
{
	/// <summary>Provides an awaiter for a <see cref="ValueTask{TResult}"/>.</summary>
	public readonly struct ValueTaskAwaiter<TResult> : ICriticalNotifyCompletion, IEquatable<ValueTaskAwaiter<TResult>>
	{
		/// <summary>The value being awaited.</summary>
		private readonly ValueTask<TResult> _value;

		/// <summary>Initializes the awaiter.</summary>
		/// <param name="value">The value to be awaited.</param>
		internal ValueTaskAwaiter( ValueTask<TResult> value ) => _value = value;

		/// <summary>Gets whether the <see cref="ValueTask{TResult}"/> has completed.</summary>
		public bool IsCompleted => _value.IsCompleted;

		/// <summary>Gets the result of the ValueTask.</summary>
		public TResult GetResult() => _value.AsTask() == null ? _value.Result : _value.AsTask().GetAwaiter().GetResult();

		/// <summary>Schedules the continuation action for this ValueTask.</summary>
		public void OnCompleted( Action continuation ) => _value.AsTask().ConfigureAwait( continueOnCapturedContext: true ).GetAwaiter().OnCompleted( continuation );

		/// <summary>Schedules the continuation action for this ValueTask.</summary>
		public void UnsafeOnCompleted( Action continuation ) => _value.AsTask().ConfigureAwait( continueOnCapturedContext: true ).GetAwaiter().UnsafeOnCompleted( continuation );
		public override int GetHashCode() => _value.GetHashCode();
		public bool Equals( ValueTaskAwaiter<TResult> other ) => _value == other._value;
		public override bool Equals( object obj ) => obj is ValueTaskAwaiter<TResult> other && Equals( other );
		public static bool operator ==( ValueTaskAwaiter<TResult> x, ValueTaskAwaiter<TResult> y ) => x.Equals( y );
		public static bool operator !=( ValueTaskAwaiter<TResult> x, ValueTaskAwaiter<TResult> y ) => !x.Equals( y );
	}
}

#pragma warning restore OverrideEquals // Structs should override Equals()
#pragma warning restore HBStructImplementIEquatable // Structs should implement IEquatable<T>
#pragma warning restore ImplementOperatorExclamationEqualsToken // Structs should implement operator !=
#pragma warning restore ImplementOperatorEqualsEqualsToken // Structs should implement operator ==
#pragma warning restore OverrideGetHashCode // Structs should override GetHashCode()
