<script type="text/javascript" src="~/Scripts/jquery-1.10.2.min.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $.ajax({
            url: "/StartTest/GetQuestion",
            type: "get",
            success: function (result) {
                $("#divQuestion").html(result)
            }
        })
    })
</script>

<script type="text/javascript">
    $("a#next").bind('click', function () {
        var radioButtons = document.getElementsByName("option");
        if (radioButtons === null) {
            $.ajax({
                url: "/StartTest/NextQuestion",
                type: "post",
                success: function (result) {
                    $("#divQuestion").html(result);
                }
            })
        }
        else {
            for (var i = 0; i < radioButtons.length; i++)
                if (radioButtons[i].checked) {
                    //document.getElementById("selectedOption").value = radioButtons[i].value;

                    //var option = radioButtons[i].value;
                    $.ajax({
                        url: "/StartTest/NextQuestion",
                        type: "post",
                        data: { option: radioButtons[i].value }
                    })
                }
        }
        //RunAction();
    });

    //function RunAction() {
    //    $.ajax({
    //        url: "/StartTest/NextQuestion",
    //        type: "post",
    //        data: document.getElementById("selectedOption").value,
    //        success: alert(document.getElementById("selectedOption").value)
    //    });


        //var action = '<%= Url.Action("NextQuestion", "StartTest") %>';
        //var data = $("#selectedOption").serialize();
        //$.get(action, data);
    //}
</script>

@*<script type="text/javascript">
    $(document).ready(function () {
        //$("divQuestion").load('/StartTest/GetQuestion');
        $.get("/StartTest/GetQuestion", "", function (response) {
            $("#divQuestion").html(response);
        })
    });
</script>*@