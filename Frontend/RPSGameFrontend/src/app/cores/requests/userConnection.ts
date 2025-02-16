import ChatUser from "./UserWithStatistics"

export interface UserConnection {
    userDTO: ChatUser;
    chatRoom: string;
}