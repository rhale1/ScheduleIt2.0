//included in fullcalendar bundle
$(function () {
    $('#calendar').fullCalendar({
//setting header properties 
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,basicWeek,basicDay'
        },
    });
});