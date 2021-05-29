using System;
using System.Collections.Generic;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Controller.Engine;
using DogAgilityCompetition.Specs.Facilities;
using FluentAssertions;
using FluentAssertions.Extensions;
using JetBrains.Annotations;
using NUnit.Framework;

namespace DogAgilityCompetition.Specs.DeviceKeyHandlingSpecs
{
    /// <summary>
    /// Tests how numeric key presses on wireless remote controls result in forming a competitor number.
    /// </summary>
    [TestFixture]
    public sealed class NumberEntryFilters
    {
        [NotNull]
        private static readonly WirelessNetworkAddress ThisRemoteControl = new("AAA111");

        [NotNull]
        private static readonly WirelessNetworkAddress OtherRemoteControl = new("BBB222");

#pragma warning disable 649 // Readonly field is never assigned
        // Reason: A nullable type with value 'null' is explicitly desired here.
        [CanBeNull]
        private static readonly TimeSpan? NullTime;
#pragma warning restore 649

        [NotNull]
        private static readonly TimeSpan? OneMinute = 1.Minutes();

        #region Tests for pressing modifier keys

        [Test]
        public void When_pressing_current_competitor_it_must_start_number_building()
        {
            // Arrange
            var filter = new NumberEntryFilter();

            // Act
            using var listener = new FilterEventListener(filter);

            filter.HandleModifierKeyDown(ThisRemoteControl, RemoteKeyModifier.EnterCurrentCompetitor);

            // Assert
            listener.EventsCollected.Should().HaveCount(1);
            listener.EventsCollected[0].ShouldBeNotifyCompetitorSelecting(true);
        }

        [Test]
        public void When_pressing_next_competitor_while_entering_current_competitor_it_must_ignore()
        {
            // Arrange
            var filter = new NumberEntryFilter();
            filter.HandleModifierKeyDown(ThisRemoteControl, RemoteKeyModifier.EnterCurrentCompetitor);

            // Act
            using var listener = new FilterEventListener(filter);

            filter.HandleModifierKeyDown(ThisRemoteControl, RemoteKeyModifier.EnterNextCompetitor);

            // Assert
            listener.EventsCollected.Should().BeEmpty();
        }

        [Test]
        public void When_pressing_next_competitor_while_others_are_entering_current_competitor_it_must_ignore()
        {
            // Arrange
            var filter = new NumberEntryFilter();
            filter.HandleModifierKeyDown(OtherRemoteControl, RemoteKeyModifier.EnterCurrentCompetitor);

            // Act
            using var listener = new FilterEventListener(filter);

            filter.HandleModifierKeyDown(ThisRemoteControl, RemoteKeyModifier.EnterNextCompetitor);

            // Assert
            listener.EventsCollected.Should().BeEmpty();
        }

        [Test]
        public void When_pressing_next_competitor_while_others_are_entering_next_competitor_it_must_join_number_building()
        {
            // Arrange
            var filter = new NumberEntryFilter();
            filter.HandleModifierKeyDown(OtherRemoteControl, RemoteKeyModifier.EnterNextCompetitor);
            filter.HandleModifierKeyDown(ThisRemoteControl, RemoteKeyModifier.EnterNextCompetitor);

            // Act
            using var listener = new FilterEventListener(filter);

            filter.HandleKeyDown(ThisRemoteControl, RemoteKey.Key7, NullTime);

            // Assert
            listener.EventsCollected.Should().HaveCount(1);
            listener.EventsCollected[0].ShouldBeNotifyDigitReceived(false, 7);
        }

        #endregion

        #region Tests for pressing keys

        [Test]
        public void When_pressing_digit_only_key_while_number_building_is_active_it_must_accept_digit()
        {
            // Arrange
            var filter = new NumberEntryFilter();
            filter.HandleModifierKeyDown(ThisRemoteControl, RemoteKeyModifier.EnterCurrentCompetitor);

            // Act
            using var listener = new FilterEventListener(filter);

            filter.HandleKeyDown(ThisRemoteControl, RemoteKey.Key7, NullTime);

            // Assert
            listener.EventsCollected.Should().HaveCount(1);
            listener.EventsCollected[0].ShouldBeNotifyDigitReceived(true, 7);
        }

        [Test]
        public void When_pressing_digit_only_key_while_others_are_building_a_different_number_it_must_ignore()
        {
            // Arrange
            var filter = new NumberEntryFilter();
            filter.HandleModifierKeyDown(OtherRemoteControl, RemoteKeyModifier.EnterNextCompetitor);
            filter.HandleModifierKeyDown(ThisRemoteControl, RemoteKeyModifier.EnterCurrentCompetitor);

            // Act
            using var listener = new FilterEventListener(filter);

            filter.HandleKeyDown(ThisRemoteControl, RemoteKey.Key4, NullTime);

            // Assert
            listener.EventsCollected.Should().BeEmpty();
        }

        [Test]
        public void When_pressing_digit_only_key_while_compatible_with_number_entry_of_others_it_must_accept_digit()
        {
            // Arrange
            var filter = new NumberEntryFilter();
            filter.HandleModifierKeyDown(OtherRemoteControl, RemoteKeyModifier.EnterNextCompetitor);
            filter.HandleModifierKeyDown(ThisRemoteControl, RemoteKeyModifier.EnterCurrentCompetitor);
            filter.HandleModifierKeyDown(ThisRemoteControl, RemoteKeyModifier.EnterNextCompetitor);

            // Act
            using var listener = new FilterEventListener(filter);

            filter.HandleKeyDown(ThisRemoteControl, RemoteKey.Key4, NullTime);

            // Assert
            listener.EventsCollected.Should().HaveCount(1);
            listener.EventsCollected[0].ShouldBeNotifyDigitReceived(false, 4);
        }

        [Test]
        public void When_pressing_digit_only_key_while_number_building_is_not_active_it_must_ignore()
        {
            // Arrange
            var filter = new NumberEntryFilter();

            // Act
            using var listener = new FilterEventListener(filter);

            filter.HandleKeyDown(ThisRemoteControl, RemoteKey.Key4, NullTime);

            // Assert
            listener.EventsCollected.Should().BeEmpty();
        }

        [Test]
        public void When_pressing_command_only_key_while_number_building_is_active_it_must_accept_command()
        {
            // Arrange
            var filter = new NumberEntryFilter();
            filter.HandleModifierKeyDown(ThisRemoteControl, RemoteKeyModifier.EnterCurrentCompetitor);

            // Act
            using var listener = new FilterEventListener(filter);

            filter.HandleKeyDown(ThisRemoteControl, RemoteKey.PassStart, OneMinute);

            // Assert
            listener.EventsCollected.Should().HaveCount(1);
            listener.EventsCollected[0].ShouldBeNotifyUnknownAction(ThisRemoteControl, RemoteKey.PassStart, OneMinute);
        }

        [Test]
        public void When_pressing_command_only_key_while_others_are_building_a_number_it_must_accept_command()
        {
            // Arrange
            var filter = new NumberEntryFilter();
            filter.HandleModifierKeyDown(OtherRemoteControl, RemoteKeyModifier.EnterNextCompetitor);

            // Act
            using var listener = new FilterEventListener(filter);

            filter.HandleKeyDown(ThisRemoteControl, RemoteKey.ResetRun, NullTime);

            // Assert
            listener.EventsCollected.Should().HaveCount(1);
            listener.EventsCollected[0].ShouldBeNotifyUnknownAction(ThisRemoteControl, RemoteKey.ResetRun, NullTime);
        }

        [Test]
        public void When_pressing_command_only_key_while_number_building_is_not_active_it_must_accept_command()
        {
            // Arrange
            var filter = new NumberEntryFilter();

            // Act
            using var listener = new FilterEventListener(filter);

            filter.HandleKeyDown(ThisRemoteControl, RemoteKey.Ready, OneMinute);

            // Assert
            listener.EventsCollected.Should().HaveCount(1);
            listener.EventsCollected[0].ShouldBeNotifyUnknownAction(ThisRemoteControl, RemoteKey.Ready, OneMinute);
        }

        [Test]
        public void When_pressing_multi_functional_key_while_number_building_is_active_it_must_accept_digit()
        {
            // Arrange
            var filter = new NumberEntryFilter();
            filter.HandleModifierKeyDown(ThisRemoteControl, RemoteKeyModifier.EnterCurrentCompetitor);

            // Act
            using var listener = new FilterEventListener(filter);

            filter.HandleKeyDown(ThisRemoteControl, RemoteKey.Key2OrPassIntermediate, OneMinute);

            // Assert
            listener.EventsCollected.Should().HaveCount(1);
            listener.EventsCollected[0].ShouldBeNotifyDigitReceived(true, 2);
        }

        [Test]
        public void When_pressing_multi_functional_key_while_others_are_building_a_different_number_it_must_ignore()
        {
            // Arrange
            var filter = new NumberEntryFilter();
            filter.HandleModifierKeyDown(OtherRemoteControl, RemoteKeyModifier.EnterCurrentCompetitor);
            filter.HandleModifierKeyDown(ThisRemoteControl, RemoteKeyModifier.EnterNextCompetitor);

            // Act
            using var listener = new FilterEventListener(filter);

            filter.HandleKeyDown(ThisRemoteControl, RemoteKey.Key2OrPassIntermediate, OneMinute);

            // Assert
            listener.EventsCollected.Should().BeEmpty();
        }

        [Test]
        public void When_pressing_multi_functional_key_while_compatible_with_number_entry_of_others_it_must_accept_digit()
        {
            // Arrange
            var filter = new NumberEntryFilter();
            filter.HandleModifierKeyDown(OtherRemoteControl, RemoteKeyModifier.EnterCurrentCompetitor);
            filter.HandleModifierKeyDown(ThisRemoteControl, RemoteKeyModifier.EnterNextCompetitor);
            filter.HandleModifierKeyDown(ThisRemoteControl, RemoteKeyModifier.EnterCurrentCompetitor);

            // Act
            using var listener = new FilterEventListener(filter);

            filter.HandleKeyDown(ThisRemoteControl, RemoteKey.Key7, NullTime);

            // Assert
            listener.EventsCollected.Should().HaveCount(1);
            listener.EventsCollected[0].ShouldBeNotifyDigitReceived(true, 7);
        }

        [Test]
        public void When_pressing_multi_functional_key_while_number_building_is_not_active_it_must_accept_command()
        {
            // Arrange
            var filter = new NumberEntryFilter();

            // Act
            using var listener = new FilterEventListener(filter);

            filter.HandleKeyDown(ThisRemoteControl, RemoteKey.Key2OrPassIntermediate, OneMinute);

            // Assert
            listener.EventsCollected.Should().HaveCount(1);
            listener.EventsCollected[0].ShouldBeNotifyUnknownAction(ThisRemoteControl, RemoteKey.Key2OrPassIntermediate, OneMinute);
        }

        [Test]
        public void When_pressing_multi_functional_key_while_no_modifiers_are_down_but_others_are_entering_current_competitor_it_must_accept_command()
        {
            // Arrange
            var filter = new NumberEntryFilter();
            filter.HandleModifierKeyDown(OtherRemoteControl, RemoteKeyModifier.EnterCurrentCompetitor);

            // Act
            using var listener = new FilterEventListener(filter);

            filter.HandleKeyDown(ThisRemoteControl, RemoteKey.Key2OrPassIntermediate, OneMinute);

            // Assert
            listener.EventsCollected.Should().HaveCount(1);
            listener.EventsCollected[0].ShouldBeNotifyUnknownAction(ThisRemoteControl, RemoteKey.Key2OrPassIntermediate, OneMinute);
        }

        #endregion

        [Test]
        public void When_entering_multiple_digits_a_number_must_be_cooperatively_formed()
        {
            // Arrange
            var filter = new NumberEntryFilter();
            filter.HandleModifierKeyDown(OtherRemoteControl, RemoteKeyModifier.EnterCurrentCompetitor);
            filter.HandleModifierKeyDown(ThisRemoteControl, RemoteKeyModifier.EnterCurrentCompetitor);

            // Act
            using var listener = new FilterEventListener(filter);

            filter.HandleKeyDown(ThisRemoteControl, RemoteKey.Key7, NullTime);
            filter.HandleKeyDown(OtherRemoteControl, RemoteKey.Key8OrDecreaseFaults, NullTime);
            filter.HandleKeyDown(ThisRemoteControl, RemoteKey.Key9OrIncreaseFaults, NullTime);

            // Assert
            listener.EventsCollected.Should().HaveCount(3);
            listener.EventsCollected[0].ShouldBeNotifyDigitReceived(true, 7);
            listener.EventsCollected[1].ShouldBeNotifyDigitReceived(true, 78);
            listener.EventsCollected[2].ShouldBeNotifyDigitReceived(true, 789);
        }

        [Test]
        public void When_entering_more_than_four_digits_the_additional_digits_must_be_ignored()
        {
            // Arrange
            var filter = new NumberEntryFilter();
            filter.HandleModifierKeyDown(ThisRemoteControl, RemoteKeyModifier.EnterCurrentCompetitor);
            filter.HandleKeyDown(ThisRemoteControl, RemoteKey.Key1OrPlaySoundA, NullTime);
            filter.HandleKeyDown(ThisRemoteControl, RemoteKey.Key2OrPassIntermediate, NullTime);
            filter.HandleKeyDown(ThisRemoteControl, RemoteKey.Key3OrToggleElimination, NullTime);
            filter.HandleKeyDown(ThisRemoteControl, RemoteKey.Key4, NullTime);

            // Act
            using var listener = new FilterEventListener(filter);

            filter.HandleKeyDown(ThisRemoteControl, RemoteKey.Key0OrMuteSound, NullTime);

            // Assert
            listener.EventsCollected.Should().BeEmpty();
        }

        #region Tests for releasing modifier keys

        [Test]
        public void When_releasing_current_competitor_without_numbers_it_must_cancel_number_building()
        {
            // Arrange
            var filter = new NumberEntryFilter();
            filter.HandleModifierKeyDown(ThisRemoteControl, RemoteKeyModifier.EnterCurrentCompetitor);

            // Act
            using var listener = new FilterEventListener(filter);

            filter.HandleModifierKeyUp(ThisRemoteControl, RemoteKeyModifier.EnterCurrentCompetitor);

            // Assert
            listener.EventsCollected.Should().HaveCount(1);
            listener.EventsCollected[0].ShouldBeNotifyCompetitorSelectCancelled(true);
        }

        [Test]
        public void When_releasing_next_competitor_it_must_end_number_building()
        {
            // Arrange
            var filter = new NumberEntryFilter();
            filter.HandleModifierKeyDown(ThisRemoteControl, RemoteKeyModifier.EnterNextCompetitor);
            filter.HandleKeyDown(ThisRemoteControl, RemoteKey.Key4, NullTime);

            // Act
            using var listener = new FilterEventListener(filter);

            filter.HandleModifierKeyUp(ThisRemoteControl, RemoteKeyModifier.EnterNextCompetitor);

            // Assert
            listener.EventsCollected.Should().HaveCount(1);
            listener.EventsCollected[0].ShouldBeNotifyCompetitorSelected(false, 4);
        }

        [Test]
        public void When_releasing_next_competitor_while_others_are_entering_current_competitor_it_must_ignore()
        {
            // Arrange
            var filter = new NumberEntryFilter();
            filter.HandleModifierKeyDown(OtherRemoteControl, RemoteKeyModifier.EnterCurrentCompetitor);
            filter.HandleModifierKeyDown(ThisRemoteControl, RemoteKeyModifier.EnterNextCompetitor);

            // Act
            using var listener = new FilterEventListener(filter);

            filter.HandleModifierKeyUp(ThisRemoteControl, RemoteKeyModifier.EnterNextCompetitor);

            // Assert
            listener.EventsCollected.Should().BeEmpty();
        }

        [Test]
        public void When_releasing_next_competitor_while_number_entry_is_no_longer_active_it_must_ignore()
        {
            // Arrange
            var filter = new NumberEntryFilter();
            filter.HandleModifierKeyDown(OtherRemoteControl, RemoteKeyModifier.EnterNextCompetitor);
            filter.HandleModifierKeyDown(ThisRemoteControl, RemoteKeyModifier.EnterNextCompetitor);
            filter.HandleModifierKeyUp(OtherRemoteControl, RemoteKeyModifier.EnterNextCompetitor);

            // Act
            using var listener = new FilterEventListener(filter);

            filter.HandleModifierKeyUp(ThisRemoteControl, RemoteKeyModifier.EnterNextCompetitor);

            // Assert
            listener.EventsCollected.Should().BeEmpty();
        }

        [Test]
        public void When_releasing_current_competitor_while_others_are_building_current_competitor_number_it_must_end_number_building()
        {
            // Arrange
            var filter = new NumberEntryFilter();
            filter.HandleModifierKeyDown(OtherRemoteControl, RemoteKeyModifier.EnterCurrentCompetitor);
            filter.HandleKeyDown(OtherRemoteControl, RemoteKey.Key4, NullTime);
            filter.HandleModifierKeyDown(ThisRemoteControl, RemoteKeyModifier.EnterCurrentCompetitor);

            // Act
            using var listener = new FilterEventListener(filter);

            filter.HandleModifierKeyUp(ThisRemoteControl, RemoteKeyModifier.EnterCurrentCompetitor);

            // Assert
            listener.EventsCollected.Should().HaveCount(1);
            listener.EventsCollected[0].ShouldBeNotifyCompetitorSelected(true, 4);
        }

        #endregion

        #region Tests for missing keys

        [Test]
        public void When_no_keys_are_included_it_should_accept_command()
        {
            var filter = new NumberEntryFilter();
            TimeSpan fourMinutes = 4.Minutes();

            using var listener = new FilterEventListener(filter);

            filter.HandleMissingKey(ThisRemoteControl, fourMinutes);

            listener.EventsCollected.Should().HaveCount(1);
            listener.EventsCollected[0].ShouldBeNotifyUnknownAction(ThisRemoteControl, null, fourMinutes);
        }

        [Test]
        public void When_no_time_and_no_keys_are_included_it_should_accept_command()
        {
            var filter = new NumberEntryFilter();

            using var listener = new FilterEventListener(filter);

            filter.HandleMissingKey(ThisRemoteControl, NullTime);

            listener.EventsCollected.Should().HaveCount(1);
            listener.EventsCollected[0].ShouldBeNotifyUnknownAction(ThisRemoteControl, null, NullTime);
        }

        #endregion

        private sealed class FilterEventListener : IDisposable
        {
            [CanBeNull]
            private NumberEntryFilter source;

            [NotNull]
            [ItemNotNull]
            public List<EventArgsWithName<NumberEntryFilter>> EventsCollected { get; } = new();

            public FilterEventListener([NotNull] NumberEntryFilter source)
            {
                Guard.NotNull(source, nameof(source));

                this.source = source;

                AttachFilterHandlers();
            }

            private void SourceOnNotifyCompetitorSelecting([CanBeNull] object sender, [NotNull] CompetitorSelectionEventArgs e)
            {
                EventsCollected.Add(new EventArgsWithName<NumberEntryFilter>("NotifyCompetitorSelecting", e));
            }

            private void SourceOnNotifyDigitReceived([CanBeNull] object sender, [NotNull] CompetitorNumberSelectionEventArgs e)
            {
                EventsCollected.Add(new EventArgsWithName<NumberEntryFilter>("NotifyDigitReceived", e));
            }

            private void SourceOnNotifyCompetitorSelected([CanBeNull] object sender, [NotNull] CompetitorNumberSelectionEventArgs e)
            {
                EventsCollected.Add(new EventArgsWithName<NumberEntryFilter>("NotifyCompetitorSelected", e));
            }

            private void SourceOnNotifyCompetitorSelectCancelled([CanBeNull] object sender, [NotNull] CompetitorSelectionEventArgs e)
            {
                EventsCollected.Add(new EventArgsWithName<NumberEntryFilter>("NotifyCompetitorSelectCancelled", e));
            }

            private void SourceOnNotifyUnknownAction([CanBeNull] object sender, [NotNull] UnknownDeviceActionEventArgs e)
            {
                EventsCollected.Add(new EventArgsWithName<NumberEntryFilter>("NotifyUnknownAction", e));
            }

            public void Dispose()
            {
                if (source != null)
                {
                    DetachFilterHandlers();

                    source = null;
                }
            }

            private void AttachFilterHandlers()
            {
                if (source != null)
                {
                    source.NotifyCompetitorSelecting += SourceOnNotifyCompetitorSelecting;
                    source.NotifyDigitReceived += SourceOnNotifyDigitReceived;
                    source.NotifyCompetitorSelected += SourceOnNotifyCompetitorSelected;
                    source.NotifyCompetitorSelectCanceled += SourceOnNotifyCompetitorSelectCancelled;
                    source.NotifyUnknownAction += SourceOnNotifyUnknownAction;
                }
            }

            private void DetachFilterHandlers()
            {
                if (source != null)
                {
                    source.NotifyCompetitorSelecting -= SourceOnNotifyCompetitorSelecting;
                    source.NotifyDigitReceived -= SourceOnNotifyDigitReceived;
                    source.NotifyCompetitorSelected -= SourceOnNotifyCompetitorSelected;
                    source.NotifyCompetitorSelectCanceled -= SourceOnNotifyCompetitorSelectCancelled;
                    source.NotifyUnknownAction -= SourceOnNotifyUnknownAction;
                }
            }
        }
    }

    public static class EventArgsWithNameForNumberEntryFilterExtensions
    {
        public static void ShouldBeNotifyCompetitorSelecting([NotNull] this EventArgsWithName<NumberEntryFilter> eventArgsWithName, bool isCurrentCompetitor)
        {
            Guard.NotNull(eventArgsWithName, nameof(eventArgsWithName));

            eventArgsWithName.Name.Should().Be("NotifyCompetitorSelecting");
            eventArgsWithName.EventArgs.Should().BeOfType<CompetitorSelectionEventArgs>();

            var competitorSelectionEventArgs = (CompetitorSelectionEventArgs)eventArgsWithName.EventArgs;
            competitorSelectionEventArgs.IsCurrentCompetitor.Should().Be(isCurrentCompetitor);
        }

        public static void ShouldBeNotifyDigitReceived([NotNull] this EventArgsWithName<NumberEntryFilter> eventArgsWithName, bool isCurrentCompetitor,
            int competitorNumber)
        {
            Guard.NotNull(eventArgsWithName, nameof(eventArgsWithName));

            eventArgsWithName.Name.Should().Be("NotifyDigitReceived");
            eventArgsWithName.EventArgs.Should().BeOfType<CompetitorNumberSelectionEventArgs>();

            var competitorNumberSelectionEventArgs = (CompetitorNumberSelectionEventArgs)eventArgsWithName.EventArgs;
            competitorNumberSelectionEventArgs.IsCurrentCompetitor.Should().Be(isCurrentCompetitor);
            competitorNumberSelectionEventArgs.CompetitorNumber.Should().Be(competitorNumber);
        }

        public static void ShouldBeNotifyCompetitorSelected([NotNull] this EventArgsWithName<NumberEntryFilter> eventArgsWithName, bool isCurrentCompetitor,
            int competitorNumber)
        {
            Guard.NotNull(eventArgsWithName, nameof(eventArgsWithName));

            eventArgsWithName.Name.Should().Be("NotifyCompetitorSelected");
            eventArgsWithName.EventArgs.Should().BeOfType<CompetitorNumberSelectionEventArgs>();

            var competitorNumberSelectionEventArgs = (CompetitorNumberSelectionEventArgs)eventArgsWithName.EventArgs;
            competitorNumberSelectionEventArgs.IsCurrentCompetitor.Should().Be(isCurrentCompetitor);
            competitorNumberSelectionEventArgs.CompetitorNumber.Should().Be(competitorNumber);
        }

        public static void ShouldBeNotifyCompetitorSelectCancelled([NotNull] this EventArgsWithName<NumberEntryFilter> eventArgsWithName,
            bool isCurrentCompetitor)
        {
            Guard.NotNull(eventArgsWithName, nameof(eventArgsWithName));

            eventArgsWithName.Name.Should().Be("NotifyCompetitorSelectCancelled");
            eventArgsWithName.EventArgs.Should().BeOfType<CompetitorSelectionEventArgs>();

            var competitorSelectionEventArgs = (CompetitorSelectionEventArgs)eventArgsWithName.EventArgs;
            competitorSelectionEventArgs.IsCurrentCompetitor.Should().Be(isCurrentCompetitor);
        }

        public static void ShouldBeNotifyUnknownAction([NotNull] this EventArgsWithName<NumberEntryFilter> eventArgsWithName,
            [NotNull] WirelessNetworkAddress source, [CanBeNull] RemoteKey? key, [CanBeNull] TimeSpan? time)
        {
            Guard.NotNull(eventArgsWithName, nameof(eventArgsWithName));
            Guard.NotNull(source, nameof(source));

            eventArgsWithName.Name.Should().Be("NotifyUnknownAction");
            eventArgsWithName.EventArgs.Should().BeOfType<UnknownDeviceActionEventArgs>();

            var unknownDeviceActionEventArgs = (UnknownDeviceActionEventArgs)eventArgsWithName.EventArgs;
            unknownDeviceActionEventArgs.Source.Should().Be(source);
            unknownDeviceActionEventArgs.Key.Should().Be(key);
            unknownDeviceActionEventArgs.SensorTime.Should().Be(time);
        }
    }
}
