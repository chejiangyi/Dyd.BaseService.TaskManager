function CreateTask(id, state,taskname,doc) {
    this.id = id;
    this.state = state;
    this.success = false;
    this.taskname = taskname;
    this.doc = doc;
}

CreateTask.prototype.CheckTaskState = function () {
    var Task = this;
    this.Num = window.setInterval(function () {
        $.ajax({
            url: '/Task/CheckTaskState',
            type: "post",
            data: {
                id: Task.id,
                state: Task.state
            },
            success: function (data) {
                if (data.code == 1) {
                    Task.success = true;
                    window.clearInterval(Task.Num);
                    if (Task.state == 1) {
                        $("#Start_" + Task.id).addClass("hide");
                        $("#Stop_" + Task.id).removeClass("hide");
                    }
                    else {
                        $("#Start_" + Task.id).removeClass("hide");
                        $("#Stop_" + Task.id).addClass("hide");
                    }
                    alert(Task.taskname + ",操作成功！");
                    $('.search input').click();
                }
                else {
                    //alert(data.msg);
                }
            }
        })
    }, 1000);
}
CreateTask.prototype.DeleteInterval = function () {
    var task = this;
    window.setTimeout(function () {
        if (!task.success) {
            window.clearInterval(task.Num);
            alert(task.taskname+",操作失败！");
        }
    }, 10000);
}

CreateTask.prototype.Init = function () {
    this.CheckTaskState();
    this.DeleteInterval();
}