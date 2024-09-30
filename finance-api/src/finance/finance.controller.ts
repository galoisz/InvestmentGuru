import { Controller, Get, Query } from '@nestjs/common';
import { FinanceService } from './finance.service';
import { PriceCriteriaDto } from './dto/price-criteria.dto';

@Controller('finance')
export class FinanceController {
  constructor(private readonly financeService: FinanceService) {}

  @Get('daily-prices')
  async getDailyPrices(@Query() priceCriteriaDto: PriceCriteriaDto): Promise<any[]> {
    return this.financeService.fetchDailyPrices(priceCriteriaDto);
  }
}
