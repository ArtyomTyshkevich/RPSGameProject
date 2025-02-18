  import { RoomTypes } from "../enums/roomTypes";
  import { RoomStatuses } from "../enums/roomStatuses";

  export interface Room {
    id: string | null;
    roomType: RoomTypes;
    roomStatus: RoomStatuses;
  }