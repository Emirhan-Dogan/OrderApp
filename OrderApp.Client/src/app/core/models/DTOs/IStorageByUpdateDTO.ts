import { IAddress } from "../IAddress";
import { IAddressByUpdateDTO } from "./IAddressByUpdateDTO";

export interface IStorageByUpdateDTO{
    lastUpdatedUserId:number,
    status:boolean,
    isDeleted:boolean,
    id:number,
    name:string,
    address:IAddressByUpdateDTO
}