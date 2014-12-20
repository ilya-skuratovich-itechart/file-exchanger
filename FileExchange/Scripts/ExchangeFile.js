exchangeFile = function () {

    // приватная переменная:
    var _fileCommentAddUrl;
    var _fileId;
    var _commentInputId;
    var _commentContainewId;
    var _addFileSubscribeUrl;
    var _delFileSubscribeUrl;

    return {
        initObject: function (fileCommentAddUrl, fileId, newCommentInputId, commentsContainerId, addFileSubscribeUrl, delFileSubscribeUrl) {
            _fileCommentAddUrl = fileCommentAddUrl;
            _fileId = fileId;
            _commentInputId = newCommentInputId;
            _commentContainewId = commentsContainerId;
            _addFileSubscribeUrl = addFileSubscribeUrl;
            _delFileSubscribeUrl = delFileSubscribeUrl;
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
                    $('#' + _commentContainewId).html(data);
                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                }
            });
        }
    };
}();