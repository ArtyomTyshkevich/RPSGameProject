import ChatUser from "./chatUser"

export interface UserConnection {
    userDTO: ChatUser;
    chatRoom: string;
}