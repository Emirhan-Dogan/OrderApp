import { IAddressByCreateDTO } from "./IAddressByCreateDTO";

export interface IStorageByCreateDTO{
    createdUserId:number,
    status:boolean,
    name:string,
    address:IAddressByCreateDTO
}