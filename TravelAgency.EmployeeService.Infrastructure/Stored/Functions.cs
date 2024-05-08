namespace TravelAgency.EmployeeService.Infrastructure.Stored;
public static class Functions
{
    public static class Insert
    {
        public const string Employee =
            @"CREATE OR REPLACE FUNCTION insert_employee_data
					(email VARCHAR, first_name VARCHAR, last_name VARCHAR, travel_agency_id INTEGER, ammount MONEY, employee_type INTEGER, 
						custom_employee_type VARCHAR, identifier VARCHAR, category_ids INTEGER [], created TIMESTAMP, created_by VARCHAR)
				RETURNS INTEGER
				LANGUAGE plpgsql
				AS $$
				DECLARE 
					employee_id INTEGER;
					salary_id INTEGER;
					driving_licence_id INTEGER := NULL;
				BEGIN
					INSERT INTO salary (ammount, created, created_by) 
							VALUES (ammount, created, created_by);

					SELECT currval(pg_get_serial_sequence('salary','id')) INTO salary_id;

					IF identifier IS NOT NULL THEN 
						SELECT insert_driving_licence_data(identifier, category_ids, created, created_by) INTO driving_licence_id;
					END IF;
	
					INSERT INTO employee (first_name, last_name, email, salary_id, travel_agency_id, employee_type, custom_employee_type, driving_licence_id, created, created_by)
							VALUES (first_name, last_name, email, currval(pg_get_serial_sequence('salary','id')), travel_agency_id, employee_type, custom_employee_type, driving_licence_id, created, created_by);

					SELECT currval(pg_get_serial_sequence('employee','id')) INTO employee_id;

					RETURN employee_id;
				END; $$;";

        public const string DrivingLicence =
            @"CREATE OR REPLACE FUNCTION insert_driving_licence_data (identifier VARCHAR, category_ids INTEGER ARRAY, created TIMESTAMP, created_by VARCHAR)
			RETURNS INTEGER
			LANGUAGE plpgsql
			AS $$
			DECLARE 
				driving_licence_id INTEGER;
				item INTEGER;
			BEGIN
				INSERT INTO driving_licence (identifier, created, created_by)
					VALUES (identifier, created, created_by);

				SELECT currval(pg_get_serial_sequence('driving_licence','id')) INTO driving_licence_id;

				FOREACH item IN ARRAY category_ids
				LOOP
					INSERT INTO driving_licence_category (driving_licence_id, category_id, created, created_by)
						VALUES (driving_licence_id, item, created, created_by);
				END LOOP;
	
				RETURN driving_licence_id;
			END; $$;";

		public const string Category =
			@"CREATE OR REPLACE FUNCTION insert_category (name VARCHAR, created TIMESTAMP, created_by VARCHAR)
				RETURNS INTEGER
				LANGUAGE plpgsql
				AS $$
				DECLARE 
					category_id INTEGER;
				BEGIN
					INSERT INTO category (name, created, created_by) 
							VALUES (name, created, created_by);

					SELECT currval(pg_get_serial_sequence('category','id')) INTO category_id;

					RETURN category_id;
				END; $$;";
    }

}
