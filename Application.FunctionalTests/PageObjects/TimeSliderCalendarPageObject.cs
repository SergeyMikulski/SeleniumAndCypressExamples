using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FunctionalTests.PageObjects
{
    public class TimeSliderCalendarPageObject : PagesPageObjectBase
    {
        public TimeSliderCalendarPageObject(IAutomateBrowser browser) : base(browser)
        {
        }

        private IDomElement TimeSliderCalendarContainer => RightContainer.ElementFinder.WithClass("date-range-slider").FindFirstOrDefault();

        #region CalendarInputFields
        private IDomElement CalendarFieldsContainer => TimeSliderCalendarContainer.ElementFinder.WithClass("date-range-picker").FindFirstOrDefault();

        private IDomElement CalendarFromInputField => CalendarFieldsContainer.ElementFinder.WithId("cui-input-0").FindFirstOrDefault();

        private IDomElement CalendarFromInputButton => CalendarFromInputField.Parent.ElementFinder.WithClass("calendar").FindFirstOrDefault();

        private IDomElement CalendarToInputField => CalendarFieldsContainer.ElementFinder.WithId("cui-input-1").FindFirstOrDefault();

        private IDomElement CalendarToInputButton => CalendarToInputField.Parent.ElementFinder.WithClass("calendar").FindFirstOrDefault();

        public DateTime CalendarFromInputFieldDate => DateTime.Parse(CalendarFromInputField["value"], CultureInfo.InvariantCulture);

        public void CalendarFromInputButtonClick()
        {
            CalendarFromInputButton.Click();
            TimeSliderCalendarPageObject timeSliderCalendarPageObject = new TimeSliderCalendarPageObject(Browser);
            WaitCalendarContentDisplayed();
            WaitCalendarSelectedCellDisplayed();
        }

        public DateTime CalendarToInputFieldDate => DateTime.Parse(CalendarToInputField["value"], CultureInfo.InvariantCulture);

        public void CalendarToInputButtonClick()
        {
            CalendarToInputButton.Click();
            TimeSliderCalendarPageObject timeSliderCalendarPageObject = new TimeSliderCalendarPageObject(Browser);
            WaitCalendarContentDisplayed();
            WaitCalendarSelectedCellDisplayed();
        }
        #endregion

        #region Calendar

        private IDomElement CalendarContainer => Browser.ElementFinder.WithTagName("cui-popover-container-window").FindFirstOrDefault();

        private IDomElement CalendarHeader => CalendarContainer.ElementFinder.WithClass("cui-calendar-header").FindFirstOrDefault();

        private IDomElement CalendarHeaderLeftArrow => CalendarHeader.ElementFinder.WithClass("left").FindFirstOrDefault();

        private IDomElement CalendarHeaderRightArrow => CalendarHeader.ElementFinder.WithClass("right").FindFirstOrDefault();

        private IDomElement CalendarHeaderDateField => CalendarHeader.ElementFinder.WithTagName("span").FindFirstOrDefault();

        private IDomElement CalendarContent => CalendarContainer.ElementFinder.WithClass("cui-calendar-content").FindFirstOrDefault();

        public void WaitCalendarContentDisplayed() => Browser.Wait(TimeSpan.FromSeconds(5), x => CalendarContent.Displayed);

        private IEnumerable<IDomElement> CalendarEnabledCells => CalendarContent.ElementFinder.WithTagName("div")
            .WithClass("cui-calendar-body-cell-content")
            .WithoutClass("cui-calendar-body-disabled").Find();

        private IEnumerable<IDomElement> CalendarDisabledCells => CalendarContent.ElementFinder.WithTagName("div")
            .WithClass("cui-calendar-body-cell-content")
            .WithClass("cui-calendar-body-disabled").Find();

        private IDomElement CalendarSelectedCell => CalendarContent.ElementFinder.WithClass("cui-calendar-body-selected").FindFirstOrDefault();

        public void WaitCalendarSelectedCellDisplayed() => Browser.Wait(TimeSpan.FromSeconds(3), x => CalendarSelectedCell.Displayed);

        public string CalendarSelectedCellText => CalendarSelectedCell.Text;

        public void CalendarHeaderLeftArrowClick() => CalendarHeaderLeftArrow.Click();

        public void CalendarHeaderRightArrowClick() => CalendarHeaderRightArrow.Click();

        public void CalendarHeaderDateFieldClick() => CalendarHeaderDateField.Click();

        public string CalendarHeaderDateFieldText => CalendarHeaderDateField.Text;

        public void SetCalendarDate(DateTime date)
        {
            ChooseYear(date.Year.ToString());
            ChooseMonth(date.ToString("MMM", CultureInfo.InvariantCulture));
            ChooseDay(date.Day.ToString());
            Browser.WaitForAjax();
            WaitMaskDisappear();
        }

        private void ChooseYear(string year)
        {
            CalendarHeaderDateFieldClick();
            Browser.WaitForAjax();

            if (CalendarEnabledCells.Where(x => x.Text == year).Count() > 0)
                CalendarEnabledCells.Single(x => x.Text == year).Click();
            else
            {
                GetDateRangeNeeded(int.Parse(year));
                CalendarEnabledCells.Single(x => x.Text == year).Click();
            }
        }

        private void GetDateRangeNeeded(int year)
        {
            if (MaxYearAvailable() < year)
                CalendarHeaderRightArrowClick();

            if (MinYearAvailable() > year)
                CalendarHeaderLeftArrowClick();
        }

        private int MaxYearAvailable()
        {
            var year = 0;
            foreach (var y in CalendarEnabledCells)
            {
                if (int.Parse(y.Text) > year)
                    year = int.Parse(y.Text);
            }
            return year;
        }

        private int MinYearAvailable()
        {
            var year = 10000;
            foreach (var y in CalendarEnabledCells)
            {
                if (int.Parse(y.Text) < year)
                    year = int.Parse(y.Text);
            }
            return year;
        }

        private void ChooseMonth(string month)
        {
            CalendarEnabledCells.Single(x => x.Text == month.ToUpper()).Click();
        }

        private void ChooseDay(string day)
        {
            CalendarEnabledCells.Single(x => x.Text == day).Click();
        }

        public bool IsYearUnAvailable(DateTime unAvailableDate, DateTime availableDate)
        {
            bool result;
            CalendarHeaderDateFieldClick();
            Browser.WaitForAjax();

            if (CalendarEnabledCells.Where(x => x.Text == availableDate.Year.ToString()).Count() > 0)
            {
                result = UnAvailableYearCheck(unAvailableDate);
                CalendarEnabledCells.Single(x => x.Text == availableDate.Year.ToString()).Click();
            }
            else
            {
                GetDateRangeNeeded(int.Parse(availableDate.Year.ToString()));
                result = UnAvailableYearCheck(unAvailableDate);
                CalendarEnabledCells.Single(x => x.Text == availableDate.Year.ToString()).Click();
            }
            return result;
        }

        private bool UnAvailableYearCheck(DateTime unAvailableDate)
        {
            List<string> yearList = new List<string>();
            foreach (var item in CalendarDisabledCells)
            {
                yearList.Add(item.Text);
            }

            var result = yearList.Contains(unAvailableDate.Year.ToString());
            return result;
        }

        public bool IsMonthUnAvailable(DateTime unAvailableDate, DateTime availableDate, bool maxDateFlag = false)
        {
            var result = true;

            if ((availableDate.Month != 1) && !maxDateFlag)
            {
                result = CheckUnavailableDate(unAvailableDate);
            }

            if ((availableDate.Month != 12) && maxDateFlag)
            {
                result = CheckUnavailableDate(unAvailableDate);
            }

            CalendarEnabledCells.Single(x => x.Text == availableDate.ToString("MMM", CultureInfo.InvariantCulture).ToUpper()).Click();

            ChooseDay(availableDate.Day.ToString());
            Browser.WaitForAjax();
            WaitMaskDisappear();

            return result;
        }

        private bool CheckUnavailableDate(DateTime unAvailableDate)
        {
            List<string> monthList = new List<string>();
            foreach (var item in CalendarDisabledCells)
            {
                monthList.Add(item.Text);
            }
            var result = monthList.Contains(unAvailableDate.ToString("MMM", CultureInfo.InvariantCulture).ToUpper());

            return result;
        }
        #endregion

        #region TimeSlider
        private IDomElement MinimalDateLabel => TimeSliderCalendarContainer.ElementFinder.WithClass("date-picker-label-above-left").FindFirstOrDefault();

        private IDomElement MaximalDateLabel => TimeSliderCalendarContainer.ElementFinder.WithClass("date-picker-label-above-right").FindFirstOrDefault();

        public DateTime MinimalDateAvailable => DateTime.Parse(MinimalDateLabel.Text);

        public string MinimalDateAvailableText => MinimalDateLabel.Text;

        public DateTime MaximalDateAvailable => DateTime.Parse(MaximalDateLabel.Text);

        public string MaximalDateAvailableText => MaximalDateLabel.Text;

        public string AppliedDateInCalendarText => CalendarSelectedCellText + CalendarHeaderDateFieldText;

        public DateTime AppliedDateInCalendar => DateTime.Parse(AppliedDateInCalendarText);

        private IDomElement TimeSliderLeftToolTip => Browser.ElementFinder.WithId("cui-tooltip-1").FindFirstOrDefault();

        private IDomElement TimeSliderRightToolTip => Browser.ElementFinder.WithId("cui-tooltip-2").FindFirstOrDefault();

        public DateTime TimeSliderLeftToolTipDate => DateTime.Parse(TimeSliderLeftToolTip.Text, CultureInfo.InvariantCulture);

        public DateTime TimeSliderRightToolTipDate => DateTime.Parse(TimeSliderRightToolTip.Text, CultureInfo.InvariantCulture);


        private IDomElement TimeSliderLeftDragItem => TimeSliderCalendarContainer.ElementFinder.WithTagName("span").WithAttributeValue("aria-describedby", "cui-tooltip-1").FindFirstOrDefault();

        private IDomElement TimeSliderRightDragItem => TimeSliderCalendarContainer.ElementFinder.WithTagName("span").WithAttributeValue("aria-describedby", "cui-tooltip-2").FindFirstOrDefault();

        private IDomElement TimeSliderGrabRangeButton => TimeSliderCalendarContainer.ElementFinder.WithTagName("button").WithClass("grab-range").FindFirstOrDefault();

        public void DragAndDropLeftToolTip(int pixelNumber)
        {
            TimeSliderLeftDragItem.DragAndDropToOffset(pixelNumber, 0);
            Browser.WaitForAjax();
            WaitMaskDisappear();
        }

        public void DragAndDropRightToolTip(int pixelNumber)
        {
            TimeSliderRightDragItem.DragAndDropToOffset(pixelNumber, 0);
            Browser.WaitForAjax();
            WaitMaskDisappear();
        }

        public void DragAndDropByGrabRangeButton(int pixelNumber)
        {
            TimeSliderGrabRangeButton.DragAndDropToOffset(pixelNumber, 0);
            Browser.WaitForAjax();
            WaitMaskDisappear();
        }
        #endregion
    }
}
