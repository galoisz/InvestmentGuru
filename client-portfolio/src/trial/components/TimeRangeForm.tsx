// src/components/TimeRangeForm.tsx
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
  IconButton,
} from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import { useFormik } from "formik";
import * as Yup from "yup";
import { isBefore, isAfter, parseISO, isWithinInterval, format } from "date-fns";

interface TimeRange {
  startDate: string;
  endDate: string;
}


const validationSchema = Yup.object({
    startDate: Yup.date()
      .required("Start date is required")
      .transform((value, originalValue) => {
        return typeof originalValue === "string" ? parseISO(originalValue) : value;
      }),
    endDate: Yup.date()
      .required("End date is required")
      .transform((value, originalValue) => {
        return typeof originalValue === "string" ? parseISO(originalValue) : value;
      })
      .test("is-after-start", "End date must be after start date", function (value) {
        const { startDate } = this.parent;
        // Only run the test if both dates are valid
        return startDate && value ? isAfter(value, startDate) : true;
      }),
  });

const TimeRangeForm: React.FC = () => {
  const [timeRanges, setTimeRanges] = useState<TimeRange[]>([]);
  const [formError, setFormError] = useState<string | null>(null);

  const formik = useFormik({
    initialValues: {
      startDate: "",
      endDate: "",
    },
    validationSchema,
    onSubmit: (values, { resetForm }) => {
      const formattedRange: TimeRange = {
        startDate: format(parseISO(values.startDate), "yyyy-MM-dd"),
        endDate: format(parseISO(values.endDate), "yyyy-MM-dd"),
      };

      // Check for overlapping ranges
      const isOverlap = timeRanges.some((range) =>
        isWithinInterval(parseISO(formattedRange.startDate), {
          start: parseISO(range.startDate),
          end: parseISO(range.endDate),
        }) ||
        isWithinInterval(parseISO(formattedRange.endDate), {
          start: parseISO(range.startDate),
          end: parseISO(range.endDate),
        })
      );

      if (isOverlap) {
        setFormError("Time ranges cannot overlap.");
        return;
      }

      setTimeRanges([...timeRanges, formattedRange]);
      resetForm();
      setFormError(null); // Clear any existing error
    },
  });

  const handleRemoveRange = (index: number) => {
    setTimeRanges((prevRanges) => prevRanges.filter((_, i) => i !== index));
  };

  return (
    <Box>
      <Typography variant="h4" gutterBottom>
        Add Time Range
      </Typography>
      <form onSubmit={formik.handleSubmit}>
        <TextField
          fullWidth
          margin="normal"
          label="Start Date"
          name="startDate"
          type="date"
          InputLabelProps={{ shrink: true }}
          value={formik.values.startDate}
          onChange={formik.handleChange}
          error={formik.touched.startDate && Boolean(formik.errors.startDate)}
          helperText={formik.touched.startDate && formik.errors.startDate}
        />
        <TextField
          fullWidth
          margin="normal"
          label="End Date"
          name="endDate"
          type="date"
          InputLabelProps={{ shrink: true }}
          value={formik.values.endDate}
          onChange={formik.handleChange}
          error={formik.touched.endDate && Boolean(formik.errors.endDate)}
          helperText={formik.touched.endDate && formik.errors.endDate}
        />
        <Button
          type="submit"
          variant="contained"
          color="primary"
          sx={{ mt: 2 }}
        >
          Add Time Range
        </Button>
        {formError && (
          <FormHelperText error sx={{ mt: 1 }}>
            {formError}
          </FormHelperText>
        )}
      </form>

      <Typography variant="h6" sx={{ mt: 4 }}>
        Added Time Ranges
      </Typography>

      <TableContainer component={Paper} sx={{ mt: 2 }}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Start Date</TableCell>
              <TableCell>End Date</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {timeRanges.map((range, index) => (
              <TableRow key={index}>
                <TableCell>{range.startDate}</TableCell>
                <TableCell>{range.endDate}</TableCell>
                <TableCell>
                  <IconButton
                    color="secondary"
                    onClick={() => handleRemoveRange(index)}
                  >
                    <DeleteIcon />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );
};

export default TimeRangeForm;
