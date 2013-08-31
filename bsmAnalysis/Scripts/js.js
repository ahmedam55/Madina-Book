
function likeList() {
    $(".head").mouseover(function () {
        $(this).attr("title", function () {
            var ti = "";
            $(this).children().each(function () {
                ti += $(this).text() + ",";

            });
            return ti;
        }
                                );
    })
};
function autoResize(e) {
    var ele = e.target;
    var t = ele.scrollTop;
    ele.scrollTop = 0
    
    if (t > 0) {
        ele.style.overflowY = "hidden";
        ele.style.height = (ele.offsetHeight + t + t) + "px";
    }


}
//angular.module('ang', []).
//  config(function ($routeProvider) {
//      $routeProvider.
//        when('/:g', { controller: ang }).
//        otherwise({ redirectTo: '/' });
//  });

//function profileController($scope) {


//    $.get('/Home/getProfileData/1', function (data) {
//        $scope.data =data;
//        alert($scope.data.length);
//    });

//}
var myApp = angular.module('myApp', ['ngSanitize']);
function myController($scope, $routeParams,$location) {//begin of angularjs $scope

    

    $scope.shareMessage = function (post) {
        $.get('/Home/shareMessage/' + post.MessageId, function (data) {

            post.usersShared = data;
            $scope.$apply();
        })
    }
    $scope.profileData;
    $scope.profilePage = function (userId) {


        $.get('/Home/getProfileData/'+userId, function (data) {
            $scope.profileData = data;
            $scope.$apply();
            $scope.sendMessage();
            $scope.requestFriendship();
            $scope.removeRequestFriendship();
            $scope.deleteFriend();
            $scope.requestSubscribtion();
            $scope.acceptSubscribtion();
            $scope.removeRequestSubscribtion();
            $scope.disagreeSubscribtion();
            likeList();
        });

    };
    // setTimeout(function () { var h = $("html").attr("ng-controller", "h"); angular.bootstrap(h, []); },1000)

    $scope.g="";
    $scope.$watch(function () { return $location.path() }, function () { $scope.g = $location.path(); });
    $scope.$watch(function () { return $scope.g; }, function () {
        
        if ($scope.g[1] == "g" )
        {var sp = $scope.g.split('g');}
        if ($.isNumeric(sp[1]))
        {
            //alert(sp[1]);
            get("http://localhost:28992/Home/getGroupMessages?groupId=" + sp[1]);
            $('#share').unbind();
           $scope.addMessageInGroup(sp[1]);
        }
       
    });

    $scope.results = [];
    $scope.query = "";
    $scope.$watch("query", function () {
        $.getJSON("http://localhost:28992/Home/search", { query: $scope.query }, function (data) {
            $scope.results = data;
            $scope.$apply();
            $scope.sendMessage();
            $scope.requestFriendship();
            $scope.removeRequestFriendship();     
            $scope.deleteFriend();
            $scope.requestSubscribtion();
            $scope.acceptSubscribtion();
            $scope.removeRequestSubscribtion();
            $scope.disagreeSubscribtion();
          
        });
       
    });

   
        $scope.showComments = false;

        $scope.showC = function (num) {
            if (num == "kly") {
                $scope.showComments = !$scope.showComments;
            }
            else if (num > 5 && $scope.showComments == false) {
                return false;
            }
            else {
                return true;
            }
        };

        $scope.addMessageInGroup = function (groupId) {
            $("#share").click(function () {
                //$(this).hide()
                if ($("#msg").val() !== "" & $("#msg").val().trim().length !== 0) {
                    $.getJSON("http://localhost:28992/Home/addMessageInGroup", { groupId: groupId, body: $("#msg").val() }, function (data) {
                        $scope.posts.messages.push(data);
                        $("#msg").val("");
                        $scope.$apply();
                        //autosize();
                    });


                }

            });
        };
        $scope.addMessage = function () {
            $("#share").click(function () {
                //$(this).hide()
                if ($("#msg").val() !== "" & $("#msg").val().trim().length !== 0) {
                    $.getJSON("http://localhost:28992/Home/addMessage", { body: $("#msg").val() }, function (data) {
                        $scope.posts.messages.push(data);
                        $("#msg").val("");
                        $scope.$apply();
                        //autosize();
                    });


                }

            });
        };
        $scope.sendToIdNum;
        $scope.itemindex;
        $scope.sendtoid = function (id, index) {
            $scope.sendToIdNum = id;
            $scope.itemindex = index;
            //alert(id);
        };
        $scope.sendMessage = function () {
            $("#sendMessageButton").click(function () {
                //$(this).hide()
                if ($("#messageBody").val() !== "" & $("#messageBody").val().trim().length !== 0) {

                    $.getJSON("http://localhost:28992/Home/sendMessage", { body: $("#messageBody").val(), userid: $scope.sendToIdNum }, function (data) {

                        $("#messageBody").val("(♥)تم الإرسال(♥) ");
                        setTimeout(function () { $("#messageBody").val("") }, 3000);


                    });


                }

            });
        };
        $scope.acceptFriendship = function (userId,itemIndex) {           
                $.getJSON("http://localhost:28992/Home/acceptFriendship", { userid:userId }, function (data) {
                    $scope.requests.splice($scope.requests.indexOf(itemIndex), 1);
                    $scope.$apply();
                });
           
        }
        $scope.deleteFriend = function () {
            $(".deleteFriend").click(function () {
                $.getJSON("http://localhost:28992/Home/deleteFriend", { userid: $scope.sendToIdNum }, function (data) {

                });
            });
        }
        $scope.removeRequestFriendship = function () {
            $(".removeRequestFriendship").click(function () {
                $.getJSON("http://localhost:28992/Home/removeRequestFriendship", { userid: $scope.sendToIdNum }, function (data) {

                });
            });
        }

        $scope.requestFriendship = function () {
            $(".requestFriendship").click(function () {

                $.getJSON("http://localhost:28992/Home/requestFriendship", { userid: $scope.sendToIdNum }, function (data) {
                    $(this).val("(♥)تم الإرسال(♥) ");
                });
            });
        };
///////////////////groups membership managing////////////////////////////////////////////////////////////////////////////
        $scope.acceptSubscribtion = function (userId,groupId,index) {
            $.getJSON("http://localhost:28992/Home/acceptSubscribtion", { userId: userId, groupId: groupId }, function (data) {
                $scope.posts.requestingMembers.splice($scope.posts.requestingMembers.indexOf(index), 1);
                    $scope.$apply();
                });
        }
        $scope.disagreeSubscribtion = function (userId, groupId, index) {
          
                $.getJSON("http://localhost:28992/Home/disagreeSubscribtion", { userId: userId, groupId: groupId }, function (data) {
                    $scope.posts.members.splice($scope.posts.members.indexOf(index), 1);
                    $scope.$apply();
                });
          
        }
        $scope.leaveGroup = function (groupId, index) {

            $.getJSON("http://localhost:28992/Home/leaveGroup", {groupId: groupId }, function (data) {
                $scope.posts.members.splice($scope.posts.members.indexOf(index), 1);
                $scope.$apply();
            });

        }
        $scope.removeRequestSubscribtion = function () {
            $(".removeRequestSubscribtion").click(function () {
                $.getJSON("http://localhost:28992/Home/removeRequestSubscribtion", { groupId: $scope.sendToIdNum }, function (data) {

                });
            });
        }

        $scope.requestSubscribtion = function () {
            $(".requestSubscribtion").click(function () {

                $.getJSON("http://localhost:28992/Home/requestSubscribtion", { groupId: $scope.sendToIdNum }, function (data) {
                    $(this).val("(♥)تم الإرسال(♥) ");
                });
            });
        };//end of group membership
        $scope.deleteMessage = function (post) {

            $.getJSON("http://localhost:28992/Home/deleteMessage", { id: post.MessageId }, function (data) {
                if (data == true) {
                    $scope.posts.messages.splice($scope.posts.messages.indexOf(post), 1);
                    $scope.$apply();
                }
            });
        }
        $scope.deleteComment = function (index, CommentId) {

            $.getJSON("http://localhost:28992/Home/deleteComment", { id: CommentId }, function (data) {
                if (data == true) {


                    for (var i = 0; i < $scope.posts.messages.length; i++) {
                        for (var y = 0; y < $scope.posts.messages[i].Comments.length; y++) {
                            if ($scope.posts.messages[i].Comments[y].CommentId === CommentId) {

                                $scope.posts.messages[i].Comments.splice(index, 1);
                                $scope.$apply();

                            }
                        }

                    }


                }




            });
        }
        $(function () {//document ready.........................

            $('.chatHiddable').slideUp();
          
            if ($location.path() == "") { get("http://localhost:28992/Home/getHome"); }
            $('#uploadPic').ajaxForm(function () {
                $("#uploadResult").text(":)");
            });

            $("#makeGroupButton").click(function () {
                if ($("#groupName").val() !== "" & $("#groupName").val().trim().length !== 0) {
                    $.getJSON("http://localhost:28992/Home/makeGroup", { name: $("#groupName").val() }, function (data) {
                        $("#groupName").val("(♥)تم الإنشاء(♥) ");
                        setTimeout(function () { $("#groupName").val("") }, 3000);
                       //group when created must be pushed....................
                    });
                }
            });
            $.connection.hub.logging = true;
            $.connection.hub.start();
            // Declare a proxy to reference the hub. 
            var chat = $.connection.notifiy;
            // Create a function that the hub can call to broadcast messages.
            chat.client.addMessage = function (object) {               
                $scope.posts.messages.push(object);
                $scope.$apply();

            };

            chat.client.addComment = function (object) {
                for (var i = 0; i < $scope.posts.messages.length; i++)
                {
                    if ($scope.posts.messages[i].MessageId === object.MessageId)
                        {
                            $scope.posts.messages[i].Comments.push(object);
                            $scope.$apply();
                        }
                    
                }
            };
            $scope.chatRecieverId;
            $scope.chatsenderId = $("#UserId").val();
            $scope.chatRecieverName;
            $scope.dialouge=[];
            $scope.onlineFriends = [];

            
            // Create a function that the hub can call to broadcast messages.

          
            chat.client.newOnlineFriend = function (object) {
                $scope.onlineFriends.push(object);              
                $scope.$apply();
            };
            chat.client.disconnectedFriend = function (connectionId) {
                for (var i = 0; i < $scope.onlineFriends.length; i++) {
                    if ($scope.onlineFriends[i].connectionId == connectionId)
                    {
                        $scope.onlineFriends.splice(i, 1);
                        $scope.$apply();
                    }
                }
                
            };
            $scope.chat = function (user) {
                if ($scope.chatRecieverId != user.UserId && $scope.chatsenderId != user.UserId) {
                    $scope.dialouge = [];
                    $scope.chatRecieverId = user.UserId;
                    $scope.chatRecieverName = user.UserName;
                    $('.chatHiddable').slideDown();
                }
            }

          $("#chatText").bind("keypress", function(event){ 
              if (event.which === 13 && !event.shiftKey) { event.preventDefault(); $scope.sendChat($("#chatText").val()); $("#chatText").val("").focus(); }
        });
          $scope.sendChat = function (chatText) {

              

                $.connection.hub.start().done(function () {

                    chat.server.send(chatText,$scope.chatRecieverId);
                });
          }

          chat.client.recieveChat = function (message) {
              $scope.chat(message.writer)
              $scope.dialouge.push(message);
              $scope.$apply();

          };
            $.get('/Home/getOnlineFriends', function (data) {
                $scope.onlineFriends = data;
                $scope.$apply();
            })
            
        });//end document ready.................................

        $scope.posts = [];
        $scope.conversations = [];
        $scope.requests = [];
        var gethomeorgroup;
        //if ($routeParams.GroupId = null) {
        gethomeorgroup = "http://localhost:28992/Home/getHome";
        //    }
        //else {
        //gethomeorgroup = "http://localhost:28992/Home/getGroupMessages?id="+$routeParams.GroupId;
    //}
        function get(gethomeorgroup) {
            $.getJSON(gethomeorgroup, function (data, status) {
                $scope.posts = data;
                $scope.$apply();
                likeList();
                if ($location.path() == "") { $scope.addMessage(); }//put function to #share button
                $scope.sendMessage();
                $scope.requestFriendship();
                $scope.removeRequestFriendship();               
                $scope.deleteFriend();
                $scope.acceptSubscribtion();            
                $scope.disagreeSubscribtion();

            });
        }
       

        $.getJSON("http://localhost:28992/Home/getConversations", function (data, status) {
            $scope.conversations = data;
            $scope.$apply();

        });

        $.getJSON("http://localhost:28992/Home/getRequests", function (data, status) {
            $scope.requests = data;
            $scope.$apply();

        });
       

      

}//end of angularjs $scope



myApp
    .directive('onCommentEnter', function () {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if (event.which === 13) {
                    if (element.val() !== "" & element.val().trim().length !== 0) {
                        $.getJSON("http://localhost:28992/Home/addComment", { id: scope.post.MessageId, body: element.val() }, function (data) {
                            if (scope.post.Comments != null) {
                                scope.post.Comments.push(data);
                                scope.$apply();
                            } else {
                                scope.coco = [];
                                scope.coco.push(data);
                                scope.post.Comments = scope.coco;
                                scope.$apply();
                            }

                        });

                    }
                    element.val("");
                    scope.$apply(function () {
                        scope.$eval(attrs.onEnter);
                    });

                    event.preventDefault();
                }
            });
        };
    })
//    .directive('onEnter', function() {
//  return {
//      scope: {
//          onEnter: '&'
//      },
//    link: function(scope, element) {
//      element.bind("keydown keypress", function(event) {
//          if (event.which === 13) {
//              $scope.myChatText = element.val();
//              scope.onEnter();
            
//          scope.$apply();
//        }
//      });
//    }
//  }
//})
.directive('ilike', function () {
    return function (scope, element, attrs) {
        element.bind("click", function (event) {
            $.getJSON("http://localhost:28992/Home/likeMessage", { id: scope.post.MessageId }, function (data) {

                scope.post.usersLiked = data;
                scope.$apply();
                likeList();




            });
        });
    };
})
    .directive('ideletec', function () {
        return function (scope, element, attrs) {
            element.bind("click", function (event) {
                //$.getJSON("http://localhost:28992/Home/deleteComment", { id: scope.comment.CommentId }, function (data) {
                //    if (data == true) {

                //        scope.comment.
                //        //scope.comment.usersLiked = data;
                //        scope.$apply();
                //    }




                //});
            });
        };
    })
    .directive('ilikec', function () {
        return function (scope, element, attrs) {
            element.bind("click", function (event) {
                $.getJSON("http://localhost:28992/Home/likeComment", { id: scope.comment.CommentId }, function (data) {
                
                      
                        scope.comment.usersLiked = data;
                       scope.$apply();
                       likeList();
                   




                });
            });
        };
    });
    //.directive('addMessage', function () {
    //    return function (scope, element, attrs) {
    //        element.bind("click", function (event) {
    //            $.getJSON("http://localhost:28992/Home/likeMessage", { id: scope.post.MessageId }, function (data) {

    //                scope.post.usersLiked = data;
    //                scope.$apply();
    //                likeList();




    //            });
    //        });
    //    };
    //})
    //.directive('addMessage', function () {
    //    return function (scope, element, attrs) {
    //        element.bind("click", function (event) {
    //            $.getJSON("http://localhost:28992/Home/likeMessage", { id: scope.post.MessageId }, function (data) {

    //                scope.post.usersLiked = data;
    //                scope.$apply();
    //                likeList();




    //            });
    //        });
    //    };
    //});


