var TableEditable = function () {

    return {

        //main function to initiate the module
        init: function () {
            FormSamples.init();
            function restoreRow(oTable, nRow) {
                var aData = oTable.fnGetData(nRow);
                var jqTds = $('>td', nRow);

                for (var i = 0, iLen = jqTds.length; i < iLen; i++) {
                    oTable.fnUpdate(aData[i], nRow, i, false);
                }

                oTable.fnDraw();
            }
            /*新增一行空行*/
            function addRow(oTable, nRow) {
                var aData = oTable.fnGetData(nRow);
                var jqTds = $('>td', nRow);
                jqTds[0].innerHTML = '';
                jqTds[0].style.display = "none";
                jqTds[1].innerHTML = '<input type="text" class="form-control input-small" value="' + aData[0] + '">';
                jqTds[2].innerHTML = '<input type="text" class="form-control input-small" value="' + aData[1] + '">';
                jqTds[3].innerHTML = '<a data-toggle="modal" href="#Responsive"><input type="text" class="form-control input-small" style="width:80px;" id="Provider" value="' + aData[2] + '"><input type="hidden" id="hidProvider" /></a>';
                jqTds[4].innerHTML = '<input type="text" class="form-control datefrom" style="width:200px;" value="' + aData[3] + '">';
                jqTds[5].innerHTML = '<a data-toggle="modal" href="#basic"><input type="text" class="form-control input-small" style="width:80px;" id="Solver" value="' + aData[4] + '"><input type="hidden" id="hidSolver" /></a>';
                jqTds[6].innerHTML = '';
                jqTds[7].innerHTML = '<select id="ddlstatus" class = "select2 form-control" style="width:130px;"><option value="" selected="selected">选择评审状态</option><option value="已评审">已评审</option><option value="未通过">未通过</option><option value="延期">延期</option><option value="不评审">不评审</option></select>';
                jqTds[8].innerHTML = '<input type="text" class="form-control input-small" value="' + aData[8] + '">';
                jqTds[9].innerHTML = '<input type="text" class="form-control input-small" value="' + aData[9] + '">';
                jqTds[10].innerHTML = '<input type="text" class="form-control input-small" value="' + aData[10] + '">';
                jqTds[11].innerHTML = '<a class="edit" href="">新增</a>';
                jqTds[12].innerHTML = '<a class="cancel" data-mode="new" href="">取消</a>';
                $(".datefrom").datetimepicker({ format: 'yyyy/mm/dd hh:ii:ss', autoclose: true });
            }
            /*将该行设置为编辑状态*/
            function editRow(oTable, nRow) {
                var aData = oTable.fnGetData(nRow);
                var jqTds = $('>td', nRow);
                jqTds[0].innerHTML = '<input type="text" class="form-control input-small" value="' + aData[0] + '">';
                jqTds[1].innerHTML = '<input type="text" class="form-control input-small" value="' + aData[1] + '">';
                jqTds[2].innerHTML = '<input type="text" class="form-control input-small" value="' + aData[2] + '">';
                jqTds[3].innerHTML = '<a data-toggle="modal" href="#Responsive"><input type="text" class="form-control input-small" style="width:80px;" id="Provider" value="' + aData[3] + '"><input type="hidden" id="hidProvider" /></a>';
                jqTds[4].innerHTML = '<input type="text" class="form-control datefrom" style="width:200px;" value="' + aData[4] + '">';
                jqTds[5].innerHTML = '<a data-toggle="modal" href="#basic"><input type="text" class="form-control input-small" style="width:80px;" id="Solver" value="' + aData[5] + '"><input type="hidden" id="hidSolver" /></a>';
                jqTds[6].innerHTML = '';
                jqTds[7].innerHTML = '<select id="ddlstatus" class = "select2 form-control" style="width:130px;"><option value="" selected="selected">选择评审状态</option><option value="已评审">已评审</option><option value="未通过">未通过</option><option value="延期">延期</option><option value="不评审">不评审</option></select>';
                jqTds[8].innerHTML = '<input type="text" class="form-control input-small" value="' + aData[8] + '">';
                jqTds[9].innerHTML = '<input type="text" class="form-control input-small" value="' + aData[9] + '">';
                jqTds[10].innerHTML = '<input type="text" class="form-control input-small" value="' + aData[10] + '">';
                jqTds[11].innerHTML = '<a class="edit" href="">编辑</a>';
                jqTds[12].innerHTML = '<a class="cancel" href="">取消</a>';
                $(".datefrom").datetimepicker({ format: 'yyyy/mm/dd hh:ii:ss', autoclose: true });
                var ddl = $("#ddlstatus");
                ddl.val(aData[7]);
                $.ajax({
                    url: "/Review/GetReviewProByID",
                    type: "POST",
                    data: { id: aData[0] },
                    success: function (data) {
                        var name = data.split('|');
                        $("#hidProvider").val(name[0]);
                        $("#hidSolver").val(name[1]);
                    }
                });
            }
            /*新增保存方法*/
            function saveRow(oTable, nRow) {
                var jqInputs = $('input', nRow);
                var ddl = $("#ddlstatus");
                oTable.fnUpdate('', nRow, 0, false);
                oTable.fnUpdate(jqInputs[0].value, nRow, 1, false);
                oTable.fnUpdate(jqInputs[1].value, nRow, 2, false);
                oTable.fnUpdate(jqInputs[2].value, nRow, 3, false);
                oTable.fnUpdate(jqInputs[4].value, nRow, 4, false);
                oTable.fnUpdate(jqInputs[5].value, nRow, 5, false);
                oTable.fnUpdate(ddl.val(), nRow, 6, false);
                oTable.fnUpdate(jqInputs[7].value, nRow, 7, false);
                oTable.fnUpdate(jqInputs[8].value, nRow, 8, false);
                oTable.fnUpdate(jqInputs[9].value, nRow, 9, false);
                oTable.fnUpdate('<a class="edit" href="">编辑</a>', nRow, 10, false);
                oTable.fnUpdate('<a class="delete" href="">删除</a>', nRow, 11, false);

                //添加需求
                if (jqInputs[0].value != "") {
                    $.ajax({
                        url: "/Review/InsertReproWithOutDeadLine",
                        type: "POST",
                        data: { DemandCode: jqInputs[0].value, Description: jqInputs[1].value, Provider: jqInputs[3].value, date: jqInputs[4].value, Solver: jqInputs[6].value, ifsolve: ddl.val(), DevelopTime: jqInputs[7].value, DesignTime: jqInputs[8].value, TestTime: jqInputs[9].value},
                        success: function (data) {
                            location.reload();
                            layer.msg('添加成功', 1, -1);
                        }
                    });
                }
                else {
                    alert("需求编号不能为空");
                    oTable.fnDeleteRow(nRow);
                }
            }
            /*编辑方法*/
            function updateRow(oTable, nRow) {
                var jqInputs = $('input', nRow);
                var ddl = $("#ddlstatus");
                oTable.fnUpdate(jqInputs[0].value, nRow, 0, false);
                oTable.fnUpdate(jqInputs[1].value, nRow, 1, false);
                oTable.fnUpdate(jqInputs[2].value, nRow, 2, false);
                oTable.fnUpdate(jqInputs[3].value, nRow, 3, false);
                oTable.fnUpdate(jqInputs[5].value, nRow, 4, false);
                oTable.fnUpdate(jqInputs[6].value, nRow, 5, false);
                oTable.fnUpdate('', nRow, 6, false);
                oTable.fnUpdate(ddl.val(), nRow, 7, false);
                oTable.fnUpdate(jqInputs[8].value, nRow, 8, false);
                oTable.fnUpdate(jqInputs[9].value, nRow, 9, false);
                oTable.fnUpdate(jqInputs[10].value, nRow, 10, false);
                oTable.fnUpdate('<a class="edit" href="">编辑</a>', nRow, 11, false);
                oTable.fnUpdate('<a class="delete" href="">删除</a>', nRow, 12, false);


                //编辑需求
                if (jqInputs[1].value != "") {
                    $.ajax({
                        url: "/Review/UpdateRepro",
                        type: "POST",
                        data: { id: jqInputs[0].value, DemandCode: jqInputs[1].value, Description: jqInputs[2].value, Provider: jqInputs[4].value, date: jqInputs[5].value, Solver: jqInputs[7].value, ifsolve: ddl.val() ,DevelopTime: jqInputs[8].value, DesignTime: jqInputs[9].value, TestTime: jqInputs[10].value},
                        success: function (data) {
                            //oTable.fnDraw();
                            location.reload();
                            layer.msg('编辑成功', 1, -1);
                        }
                    });
                }
                else {
                    alert("需求编号不能为空");
                    //oTable.fnDeleteRow(nRow);
                }
            }

            function cancelEditRow(oTable, nRow) {
                var jqInputs = $('input', nRow);
                oTable.fnUpdate(jqInputs[0].value, nRow, 0, false);
                oTable.fnUpdate(jqInputs[1].value, nRow, 1, false);
                oTable.fnUpdate(jqInputs[2].value, nRow, 2, false);
                oTable.fnUpdate(jqInputs[3].value, nRow, 3, false);
                oTable.fnUpdate(jqInputs[4].value, nRow, 4, false);
                oTable.fnUpdate(jqInputs[5].value, nRow, 5, false);
                oTable.fnUpdate('<a class="edit" href="">编辑</a>', nRow, 6, false);
                oTable.fnDraw();
            }

            var oTable = $('#sample_editable_1').dataTable({
                "aLengthMenu": [
                    [5, 10, 15, 20, -1],
                    [5, 10, 15, 20, "All"] // change per page values here
                ],
                // set the initial value
                "iDisplayLength": 10,

                "sPaginationType": "bootstrap",
                "oLanguage": {
                    "sLengthMenu": "_MENU_ records",
                    "oPaginate": {
                        "sPrevious": "Prev",
                        "sNext": "Next"
                    }
                },
                "aoColumnDefs": [{
                    'bSortable': false,
                    'aTargets': [0]
                }
                ]
            });

            jQuery('#sample_editable_1_wrapper .dataTables_filter input').addClass("form-control input-medium"); // modify table search input
            jQuery('#sample_editable_1_wrapper .dataTables_length select').addClass("form-control input-small"); // modify table per page dropdown
            jQuery('#sample_editable_1_wrapper .dataTables_length select').select2({
                showSearchInput: false //hide search box with special css class
            }); // initialize select2 dropdown

            var nEditing = null;

            $('#sample_editable_1_new').click(function (e) {
                e.preventDefault();
                var aiNew = oTable.fnAddData(['', '', '', '', '', '', '', '','','','',
                        '<a class="edit" href="">新增</a>', '<a class="cancel" data-mode="new" href="">取消</a>'
                ]);
                var nRow = oTable.fnGetNodes(aiNew[0]);
                addRow(oTable, nRow);
                nEditing = nRow;
            });

            $('#sample_editable_1 a.delete').live('click', function (e) {
                e.preventDefault();

                if (confirm("确定删除吗?") == false) {
                    return;
                }
                var nRow = $(this).parents('tr')[0];
                var aData = oTable.fnGetData(nRow);
                $.ajax({
                    url: "/Review/DeleteReviewPro",
                    type: "POST",
                    data: { id: aData[0] },
                    success: function (data) {
                        if (data == "1") {
                            oTable.fnDeleteRow(nRow);
                            layer.msg('删除成功', 1, -1);
                        }
                        else {
                            layer.msg('删除失败', 1, -1);
                        }
                    }
                });


                //alert("Deleted! Do not forget to do some ajax to sync with backend :)");
            });

            $('#sample_editable_1 a.cancel').live('click', function (e) {
                e.preventDefault();
                if ($(this).attr("data-mode") == "new") {
                    var nRow = $(this).parents('tr')[0];
                    oTable.fnDeleteRow(nRow);
                } else {
                    restoreRow(oTable, nEditing);
                    nEditing = null;
                }
            });




            $('#sample_editable_1 a.edit').live('click', function (e) {
                e.preventDefault();

                /* Get the row as a parent of the link that was clicked on */
                var nRow = $(this).parents('tr')[0];

                if (nEditing !== null && nEditing != nRow) {
                    /* Currently editing - but not this row - restore the old before continuing to edit mode */
                    restoreRow(oTable, nEditing);
                    editRow(oTable, nRow);
                    nEditing = nRow;
                } else if (nEditing == nRow && this.innerHTML == "新增") {
                    /* Editing this row and want to save it */
                    saveRow(oTable, nEditing);
                    nEditing = null;
                    //alert("Updated! Do not forget to do some ajax to sync with backend :)");
                } else if (nEditing == nRow && this.innerHTML == "编辑") {
                    updateRow(oTable, nEditing);
                    nEditing = null;
                } else {
                    /* No edit in progress - let's start one */
                    editRow(oTable, nRow);
                    nEditing = nRow;
                }
            });
        }

    };

} ();




