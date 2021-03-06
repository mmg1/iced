/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

#if !NO_INSTR_INFO
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Iced.Intel.InstructionInfoInternal;

namespace Iced.Intel {
	/// <summary>
	/// Extension methods
	/// </summary>
	public static class InstructionInfoExtensions {
		/// <summary>
		/// Gets the encoding, eg. legacy, VEX, EVEX, ...
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static EncodingKind Encoding(this Code code) =>
			(EncodingKind)((InfoHandlers.Data[(int)code * 2 + 1] >> (int)InfoFlags2.EncodingShift) & (uint)InfoFlags2.EncodingMask);

		/// <summary>
		/// Gets the CPU or CPUID feature flag
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[Obsolete("Use " + nameof(CpuidFeatures) + "() instead", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static CpuidFeature CpuidFeature(this Code code) => code.CpuidFeatures()[0];

		/// <summary>
		/// Gets the CPU or CPUID feature flags
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static CpuidFeature[] CpuidFeatures(this Code code) {
			var cpuidFeature = (CpuidFeatureInternal)((InfoHandlers.Data[(int)code * 2 + 1] >> (int)InfoFlags2.CpuidFeatureShift) & (uint)InfoFlags2.CpuidFeatureMask);
			return CpuidFeatureInternalData.ToCpuidFeatures[(int)cpuidFeature];
		}

		/// <summary>
		/// Gets flow control info
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static FlowControl FlowControl(this Code code) =>
			(FlowControl)((InfoHandlers.Data[(int)code * 2 + 1] >> (int)InfoFlags2.FlowControlShift) & (uint)InfoFlags2.FlowControlMask);

		/// <summary>
		/// Checks if the instruction isn't available in real mode or virtual 8086 mode
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[Obsolete("Use " + nameof(IsProtectedMode) + "() instead", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static bool ProtectedMode(this Code code) => code.IsProtectedMode();

		/// <summary>
		/// Checks if the instruction isn't available in real mode or virtual 8086 mode
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static bool IsProtectedMode(this Code code) =>
			(InfoHandlers.Data[(int)code * 2] & (uint)InfoFlags1.ProtectedMode) != 0;

		/// <summary>
		/// Checks if this is a privileged instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[Obsolete("Use " + nameof(IsPrivileged) + "() instead", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static bool Privileged(this Code code) => code.IsPrivileged();

		/// <summary>
		/// Checks if this is a privileged instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static bool IsPrivileged(this Code code) =>
			(InfoHandlers.Data[(int)code * 2] & (uint)InfoFlags1.Privileged) != 0;

		/// <summary>
		/// Checks if this is an instruction that implicitly uses the stack pointer (SP/ESP/RSP), eg. call, push, pop, ret, etc.
		/// See also <see cref="Instruction.StackPointerIncrement"/>
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[Obsolete("Use " + nameof(IsStackInstruction) + "() instead", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static bool StackInstruction(this Code code) => code.IsStackInstruction();

		/// <summary>
		/// Checks if this is an instruction that implicitly uses the stack pointer (SP/ESP/RSP), eg. call, push, pop, ret, etc.
		/// See also <see cref="Instruction.StackPointerIncrement"/>
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static bool IsStackInstruction(this Code code) =>
			(InfoHandlers.Data[(int)code * 2] & (uint)InfoFlags1.StackInstruction) != 0;

		/// <summary>
		/// Checks if it's an instruction that saves or restores too many registers (eg. fxrstor, xsave, etc).
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[Obsolete("Use " + nameof(IsSaveRestoreInstruction) + "() instead", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static bool SaveRestoreInstruction(this Code code) => code.IsSaveRestoreInstruction();

		/// <summary>
		/// Checks if it's an instruction that saves or restores too many registers (eg. fxrstor, xsave, etc).
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static bool IsSaveRestoreInstruction(this Code code) =>
			(InfoHandlers.Data[(int)code * 2] & (uint)InfoFlags1.SaveRestore) != 0;

		/// <summary>
		/// Checks if it's a jcc short or jcc near instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static bool IsJccShortOrNear(this Code code) =>
			(uint)(code - Code.Jo_rel8_16) <= (uint)(Code.Jg_rel8_64 - Code.Jo_rel8_16) ||
			(uint)(code - Code.Jo_rel16) <= (uint)(Code.Jg_rel32_64 - Code.Jo_rel16);

		/// <summary>
		/// Checks if it's a jcc near instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static bool IsJccNear(this Code code) =>
			(uint)(code - Code.Jo_rel16) <= (uint)(Code.Jg_rel32_64 - Code.Jo_rel16);

		/// <summary>
		/// Checks if it's a jcc short instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static bool IsJccShort(this Code code) =>
			(uint)(code - Code.Jo_rel8_16) <= (uint)(Code.Jg_rel8_64 - Code.Jo_rel8_16);

		/// <summary>
		/// Checks if it's a jmp short instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static bool IsJmpShort(this Code code) =>
			(uint)(code - Code.Jmp_rel8_16) <= (uint)(Code.Jmp_rel8_64 - Code.Jmp_rel8_16);

		/// <summary>
		/// Checks if it's a jmp near instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static bool IsJmpNear(this Code code) =>
			(uint)(code - Code.Jmp_rel16) <= (uint)(Code.Jmp_rel32_64 - Code.Jmp_rel16);

		/// <summary>
		/// Checks if it's a jmp short or a jmp near instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static bool IsJmpShortOrNear(this Code code) =>
			(uint)(code - Code.Jmp_rel8_16) <= (uint)(Code.Jmp_rel8_64 - Code.Jmp_rel8_16) ||
			(uint)(code - Code.Jmp_rel16) <= (uint)(Code.Jmp_rel32_64 - Code.Jmp_rel16);

		/// <summary>
		/// Checks if it's a jmp far instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static bool IsJmpFar(this Code code) =>
			(uint)(code - Code.Jmp_ptr1616) <= (uint)(Code.Jmp_ptr3216 - Code.Jmp_ptr1616);

		/// <summary>
		/// Checks if it's a call near instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static bool IsCallNear(this Code code) =>
			(uint)(code - Code.Call_rel16) <= (uint)(Code.Call_rel32_64 - Code.Call_rel16);

		/// <summary>
		/// Checks if it's a call far instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static bool IsCallFar(this Code code) =>
			(uint)(code - Code.Call_ptr1616) <= (uint)(Code.Call_ptr3216 - Code.Call_ptr1616);

		/// <summary>
		/// Checks if it's a jmp near reg/[mem] instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static bool IsJmpNearIndirect(this Code code) =>
			(uint)(code - Code.Jmp_rm16) <= (uint)(Code.Jmp_rm64 - Code.Jmp_rm16);

		/// <summary>
		/// Checks if it's a jmp far [mem] instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static bool IsJmpFarIndirect(this Code code) =>
			(uint)(code - Code.Jmp_m1616) <= (uint)(Code.Jmp_m6416 - Code.Jmp_m1616);

		/// <summary>
		/// Checks if it's a call near reg/[mem] instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static bool IsCallNearIndirect(this Code code) =>
			(uint)(code - Code.Call_rm16) <= (uint)(Code.Call_rm64 - Code.Call_rm16);

		/// <summary>
		/// Checks if it's a call far [mem] instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static bool IsCallFarIndirect(this Code code) =>
			(uint)(code - Code.Call_m1616) <= (uint)(Code.Call_m6416 - Code.Call_m1616);
	}
}
#endif
