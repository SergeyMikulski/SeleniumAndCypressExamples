
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.FunctionalTests.PageObjects;

namespace Application.FunctionalTests.Tests
{
    [LogInAs("User2")]
    [Category("TimeSliderCalendar")]
    public class TimeSliderCalendarTests : LayoutTestsBase
    {
        private TimeSliderCalendarPageObject _timeSliderCalendarPageObject;

        private DateTime _minDate;
        private DateTime _maxDate;

        [SetUp]
        public void NavigateToApplication()
        {
            _timeSliderCalendarPageObject = NavigateTo<TimeSliderCalendarPageObject>();
            Browser.WaitForAjax();
            _timeSliderCalendarPageObject.WaitMaskDisappear();
        }

        [Test]
        public void CalendarInfluenceTimeSliderAndFields()
        {
            InitialSetCalendar();

            Assert.AreEqual(_minDate, _timeSliderCalendarPageObject.CalendarFromInputFieldDate);
            Assert.AreEqual(_minDate, _timeSliderCalendarPageObject.TimeSliderLeftToolTipDate);
            Assert.AreEqual(_maxDate, _timeSliderCalendarPageObject.CalendarToInputFieldDate);
            Assert.AreEqual(_maxDate, _timeSliderCalendarPageObject.TimeSliderRightToolTipDate);
        }

        [Test]
        public void TimeSliderInfluenceCalendarAndFields()
        {
            InitialSetCalendar();

            _timeSliderCalendarPageObject.DragAndDropLeftToolTip(10);
            _timeSliderCalendarPageObject.DragAndDropRightToolTip(15);
            var minDate = _timeSliderCalendarPageObject.TimeSliderLeftToolTipDate;
            var maxDate = _timeSliderCalendarPageObject.TimeSliderRightToolTipDate;

            Assert.AreEqual(minDate, _timeSliderCalendarPageObject.CalendarFromInputFieldDate);
            Assert.AreEqual(maxDate, _timeSliderCalendarPageObject.CalendarToInputFieldDate);
        }

        [Test]
        public void TimeSliderCanBeDraggedByGrabButton()
        {
            InitialSetCalendar();
            
            _timeSliderCalendarPageObject.DragAndDropByGrabRangeButton(-10);
            Assert.IsTrue(_minDate > _timeSliderCalendarPageObject.TimeSliderLeftToolTipDate);
            Assert.IsTrue(_maxDate > _timeSliderCalendarPageObject.TimeSliderRightToolTipDate);

            _timeSliderCalendarPageObject.DragAndDropByGrabRangeButton(20);
            Assert.IsTrue(_minDate < _timeSliderCalendarPageObject.TimeSliderLeftToolTipDate);
            Assert.IsTrue(_maxDate < _timeSliderCalendarPageObject.TimeSliderRightToolTipDate);
        }

        [Test]
        public void AreCalendarFieldAndCalendarHaveTheSameDates()
        {
            var calendarFieldFromDate = _timeSliderCalendarPageObject.CalendarFromInputFieldDate;
            _timeSliderCalendarPageObject.CalendarFromInputButtonClick();
            var calendarDate = _timeSliderCalendarPageObject.AppliedDateInCalendar;
            Assert.AreEqual(calendarFieldFromDate, calendarDate);

            var calendarFieldToDate = _timeSliderCalendarPageObject.CalendarToInputFieldDate;
            _timeSliderCalendarPageObject.CalendarToInputButtonClick();
            calendarDate = _timeSliderCalendarPageObject.AppliedDateInCalendar;
            Assert.AreEqual(calendarFieldToDate, calendarDate);
        }

        [Test]
        public void AreMinMaxDatesAvailableInCalendar()
        {
            var minDateFromField = _timeSliderCalendarPageObject.MinimalDateAvailable;
            var minDate = new DateTime(minDateFromField.Year, minDateFromField.Month, DateTime.DaysInMonth(minDateFromField.Year, minDateFromField.Month));
            var maxDate = _timeSliderCalendarPageObject.MaximalDateAvailable;

            var maxUnAvailableYear = minDate.AddYears(-1);
            _timeSliderCalendarPageObject.CalendarFromInputButtonClick();
            Assert.IsTrue(_timeSliderCalendarPageObject.IsYearUnAvailable(maxUnAvailableYear, minDate));

            DateTime maxUnAvailableMonth;
            maxUnAvailableMonth = minDate.AddMonths(-1);
            Assert.IsTrue(_timeSliderCalendarPageObject.IsMonthUnAvailable(maxUnAvailableMonth, minDate));
            
            var minUnAvailableYear = maxDate.AddYears(1);
            _timeSliderCalendarPageObject.CalendarToInputButtonClick();
            Assert.IsTrue(_timeSliderCalendarPageObject.IsYearUnAvailable(minUnAvailableYear, maxDate));
            DateTime minUnAvailableMonth;
            minUnAvailableMonth = maxDate.AddMonths(1);
            Assert.IsTrue(_timeSliderCalendarPageObject.IsMonthUnAvailable(minUnAvailableMonth, maxDate, true));
        }

        private void InitialSetCalendar()
        {
            var minDate = _timeSliderCalendarPageObject.MinimalDateAvailable;
            var maxDate = _timeSliderCalendarPageObject.MaximalDateAvailable;

            var newMinDate = minDate.AddYears(1).AddMonths(2).AddDays(3);
            var newMaxDate = maxDate.AddYears(-1).AddMonths(-2).AddDays(-3);

            if (_timeSliderCalendarPageObject.CalendarFromInputFieldDate == newMinDate)
                newMinDate = newMinDate.AddDays(3);

            if (_timeSliderCalendarPageObject.CalendarToInputFieldDate == newMaxDate)
                newMaxDate = newMaxDate.AddDays(-3);

            _timeSliderCalendarPageObject.CalendarFromInputButtonClick();
            _timeSliderCalendarPageObject.SetCalendarDate(newMinDate);
            _timeSliderCalendarPageObject.CalendarToInputButtonClick();
            _timeSliderCalendarPageObject.SetCalendarDate(newMaxDate);

            _minDate = newMinDate;
            _maxDate = newMaxDate;
        }
    }
}
