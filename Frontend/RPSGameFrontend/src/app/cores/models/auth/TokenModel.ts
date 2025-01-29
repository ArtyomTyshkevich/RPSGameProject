import { RoomTypes } from "../enums/RoomTypes";
import { RoomStatuses } from "../enums/RoomStatuses";

export interface Room {
  id: string;
  roomType: RoomTypes;
  roomStatus: RoomStatuses;
}