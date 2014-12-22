exchangeFile = function () {
    var _fileCommentAddUrl;
    var _fileId;
    var _commentInputId;
    var _commentContainerId;
    var _addFileSubscribeUrl;
    var _delFileSubscribeUrl;
    var _subscribeBlockId;

    return {
        initObject: function (fileCommentAddUrl, fileId, newCommentInputId, commentsContainerId, addFileSubscribeUrl, delFileSubscribeUrl, subscribeBlockId) {
            _fileCommentAddUrl = fileCommentAddUrl;
            _fileId = fileId;
            _commentInputId = newCommentInputId;
            _commentContainerId = commentsContainerId;
            _addFileSubscribeUrl = addFileSubscribeUrl;
            _delFileSubscribeUrl = delFileSubscribeUrl;
            _subscribeBlockId = subscribeBlockId;
        },
        addComment:function() {
            var comment = $('#' + _commentInputId).val();
            if (!comment) {
                alert('comment is required');
                return;
            }
            $.ajax({
                url: _fileCommentAddUrl,
                type: 'POST',
                data: { fileId: _fileId, comment: comment},
                success: function (data) {
                    $('#' + _commentContainerId).html(data);
                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                }
            });
        },
        subscribeFileNotification: function() {
            $.ajax({
                url: _addFileSubscribeUrl,
                type: 'GET',
                dataType: 'json',
                success: function(data) {
                    if (data.Success) {
                        $('#' + _subscribeBlockId).html(data.Html);
                        alert('Success');
                    } else {
                        alert(data.Error);
                    }
                },
                error: function(xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                }
            });
        },
        unsubscribeFileNotification: function () {
            $.ajax({
                url: _delFileSubscribeUrl,
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    if (data.Success) {
                        $('#' + _subscribeBlockId).html(data.Html);
                        alert('Success');
                    } else {
                        alert(data.Error);
                    }
                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                }
            });
        }
    };
}();