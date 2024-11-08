// src/components/StockForm.tsx
import React, { useState } from "react";
import {
  Box,
  Button,
  TextField,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Typography,
  FormHelperText,
  Autocomplete,
} from "@mui/material";
import { useFormik } from "formik";
import * as Yup from "yup";
  import { observer } from "mobx-react-lite";
import portfolioStore from "../stores/portfolioStore";

interface Stock {
  name: string;
  ratio: number;
}

const validationSchema = Yup.object({
  name: Yup.string().required("Stock name is required"),
  ratio: Yup.number()
    .required("Ratio is required")
    .min(1, "Ratio must be greater than 0")
    .max(100, "Ratio must be less than or equal to 100"),
});

const StockForm: React.FC = observer(() => {
  const [stocks, setStocks] = useState<Stock[]>([]);
  const [ratioError, setRatioError] = useState<string | null>(null);

  const formik = useFormik({
    initialValues: {
      name: "",
      ratio: 0,
    },
    validationSchema,
    onSubmit: (values, { resetForm }) => {
      const newStocks = [...stocks, { name: values.name, ratio: values.ratio }];
      const totalRatio = newStocks.reduce((acc, stock) => acc + stock.ratio, 0);

      const isDuplicateName = stocks.some(
        (stock) => stock.name.toLowerCase() === values.name.toLowerCase()
      );
      if (isDuplicateName) {
        setRatioError("Stock name must be unique.");
        return;
      }

      if (totalRatio > 100) {
        setRatioError("Total ratios cannot exceed 100%. Please adjust the values.");
      } else {
        setStocks(newStocks);
        resetForm();
        setRatioError(null);
      }
    },
  });

  const totalRatio = stocks.reduce((acc, stock) => acc + stock.ratio, 0);

  return (
    <Box>
      <Typography variant="h4" gutterBottom>
        Add Stock
      </Typography>
      <form onSubmit={formik.handleSubmit}>
        <Autocomplete
          freeSolo
          options={portfolioStore.stockNames} // Use stock names from the store
          value={formik.values.name}
          onChange={(event, newValue) => formik.setFieldValue("name", newValue)}
          onInputChange={(event, newValue) => formik.setFieldValue("name", newValue)}
          renderInput={(params) => (
            <TextField
              {...params}
              fullWidth
              margin="normal"
              label="Stock Name"
              name="name"
              error={formik.touched.name && Boolean(formik.errors.name)}
              helperText={formik.touched.name && formik.errors.name}
            />
          )}
        />
        <TextField
          fullWidth
          margin="normal"
          label="Ratio (%)"
          name="ratio"
          type="number"
          value={formik.values.ratio}
          onChange={formik.handleChange}
          error={formik.touched.ratio && Boolean(formik.errors.ratio)}
          helperText={formik.touched.ratio && formik.errors.ratio}
        />
        <Button
          type="submit"
          variant="contained"
          color="primary"
          disabled={totalRatio >= 100}
          sx={{ mt: 2 }}
        >
          Add Stock
        </Button>
        {ratioError && (
          <FormHelperText error sx={{ mt: 1 }}>
            {ratioError}
          </FormHelperText>
        )}
      </form>

      <Typography variant="h6" sx={{ mt: 4 }}>
        Added Stocks (Total Ratio: {totalRatio}%)
      </Typography>

      <TableContainer component={Paper} sx={{ mt: 2 }}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Stock Name</TableCell>
              <TableCell>Ratio (%)</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {stocks.map((stock, index) => (
              <TableRow key={index}>
                <TableCell>{stock.name}</TableCell>
                <TableCell>{stock.ratio}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );
});

export default StockForm;
