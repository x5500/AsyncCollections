﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;
using System.Threading.Tasks;

#pragma warning disable OverrideGetHashCode // Structs should override GetHashCode()
#pragma warning disable ImplementOperatorEqualsEqualsToken // Structs should implement operator ==
#pragma warning disable ImplementOperatorExclamationEqualsToken // Structs should implement operator !=
#pragma warning disable HBStructImplementIEquatable // Structs should implement IEquatable<T>
#pragma warning disable OverrideEquals // Structs should override Equals()

namespace System.Runtime.CompilerServices
{
	/// <summary>Provides an awaitable type that enables configured awaits on a <see cref="ValueTask{TResult}"/>.</summary>
	/// <typeparam name="TResult">The type of the result produced.</typeparam>
	[StructLayout( LayoutKind.Auto )]
	public readonly struct ConfiguredValueTaskAwaitable<TResult> : IEquatable<ConfiguredValueTaskAwaitable<TResult>>
	{
		/// <summary>The wrapped <see cref="ValueTask{TResult}"/>.</summary>
		private readonly ValueTask<TResult> _value;
		/// <summary>true to attempt to marshal the continuation back to the original context captured; otherwise, false.</summary>
		private readonly bool _continueOnCapturedContext;

		/// <summary>Initializes the awaitable.</summary>
		/// <param name="value">The wrapped <see cref="ValueTask{TResult}"/>.</param>
		/// <param name="continueOnCapturedContext">
		/// true to attempt to marshal the continuation back to the original synchronization context captured; otherwise, false.
		/// </param>
		internal ConfiguredValueTaskAwaitable( ValueTask<TResult> value, bool continueOnCapturedContext )
		{
			_value = value;
			_continueOnCapturedContext = continueOnCapturedContext;
		}

		/// <summary>Returns an awaiter for this <see cref="ConfiguredValueTaskAwaitable{TResult}"/> instance.</summary>
		public ConfiguredValueTaskAwaiter GetAwaiter() => new ConfiguredValueTaskAwaiter( _value, _continueOnCapturedContext );

		/// <summary>Provides an awaiter for a <see cref="ConfiguredValueTaskAwaitable{TResult}"/>.</summary>
		[StructLayout( LayoutKind.Auto )]
		public readonly struct ConfiguredValueTaskAwaiter : ICriticalNotifyCompletion, IEquatable<ConfiguredValueTaskAwaiter>
		{
			/// <summary>The value being awaited.</summary>
			private readonly ValueTask<TResult> _value;
			/// <summary>The value to pass to ConfigureAwait.</summary>
			private readonly bool _continueOnCapturedContext;

			/// <summary>Initializes the awaiter.</summary>
			/// <param name="value">The value to be awaited.</param>
			/// <param name="continueOnCapturedContext">The value to pass to ConfigureAwait.</param>
			internal ConfiguredValueTaskAwaiter( ValueTask<TResult> value, bool continueOnCapturedContext )
			{
				_value = value;
				_continueOnCapturedContext = continueOnCapturedContext;
			}

			/// <summary>Gets whether the <see cref="ConfiguredValueTaskAwaitable{TResult}"/> has completed.</summary>
			public bool IsCompleted => _value.IsCompleted;

			/// <summary>Gets the result of the ValueTask.</summary>
			public TResult GetResult() => _value.AsTask() == null ? _value.Result :_value.AsTask().GetAwaiter().GetResult();

			/// <summary>Schedules the continuation action for the <see cref="ConfiguredValueTaskAwaitable{TResult}"/>.</summary>
			public void OnCompleted( Action continuation ) => _value.AsTask().ConfigureAwait( _continueOnCapturedContext ).GetAwaiter().OnCompleted( continuation );

			/// <summary>Schedules the continuation action for the <see cref="ConfiguredValueTaskAwaitable{TResult}"/>.</summary>
			public void UnsafeOnCompleted( Action continuation ) => _value.AsTask().ConfigureAwait( _continueOnCapturedContext ).GetAwaiter().UnsafeOnCompleted( continuation );
			public override int GetHashCode() => (_value, _continueOnCapturedContext).GetHashCode();
			public bool Equals( ConfiguredValueTaskAwaiter other ) => (_value, _continueOnCapturedContext) == (other._value, other._continueOnCapturedContext);
			public override bool Equals( object obj ) => obj is ConfiguredValueTaskAwaiter other && Equals( other );

			public static bool operator ==( ConfiguredValueTaskAwaiter x, ConfiguredValueTaskAwaiter y ) => x.Equals( y );
			public static bool operator !=( ConfiguredValueTaskAwaiter x, ConfiguredValueTaskAwaiter y ) => !x.Equals( y );
		}

		public override int GetHashCode() => (_value, _continueOnCapturedContext).GetHashCode();
		public bool Equals( ConfiguredValueTaskAwaitable<TResult> other ) => (_value, _continueOnCapturedContext) == (other._value, other._continueOnCapturedContext);
		public override bool Equals( object obj ) => obj is ConfiguredValueTaskAwaitable<TResult> other && Equals( other );

		public static bool operator ==( ConfiguredValueTaskAwaitable<TResult> x, ConfiguredValueTaskAwaitable<TResult> y ) => x.Equals( y );
		public static bool operator !=( ConfiguredValueTaskAwaitable<TResult> x, ConfiguredValueTaskAwaitable<TResult> y ) => !x.Equals( y );
	}
}

#pragma warning restore OverrideEquals // Structs should override Equals()
#pragma warning restore HBStructImplementIEquatable // Structs should implement IEquatable<T>
#pragma warning restore ImplementOperatorExclamationEqualsToken // Structs should implement operator !=
#pragma warning restore ImplementOperatorEqualsEqualsToken // Structs should implement operator ==
#pragma warning restore OverrideGetHashCode // Structs should override GetHashCode()
