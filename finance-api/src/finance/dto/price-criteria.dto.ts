import { IsString, IsDateString } from 'class-validator';

export class PriceCriteriaDto {
  @IsString()
  symbol: string;

  @IsDateString()
  startDate: string;

  @IsDateString()
  endDate: string;
}
